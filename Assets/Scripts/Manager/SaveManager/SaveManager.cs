using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable MemberCanBePrivate.Global

public class SaveManager : MonoBehaviour
{
    private static string Folder => UnityEngine.Application.persistentDataPath;
    private static string BaseSavePath => Path.Combine(Folder, "Save");

    private void Awake()
    {
        if (!Directory.Exists(BaseSavePath))
        {
            Directory.CreateDirectory(BaseSavePath);
        }
    }
    
    //Open folder
#if UNITY_EDITOR
    [MenuItem("Window/Save/Open Folder")]
#endif
    public static void OpenFolder()
    {
        Process.Start(BaseSavePath);
    }

    //Clear Directory
#if UNITY_EDITOR
    [MenuItem("Window/Save/Clear Folder")]
#endif
    public static void Clear()
    {
        Directory.Delete(BaseSavePath, true);
        Directory.CreateDirectory(BaseSavePath);
    }
    
    //Save T data into TModel and Serialize TModel into file
    public void SaveData<T, TModel>(T dataToSave) 
        where T : class
        where TModel : class, ISavable<T>, new()
    {
        var modelToBeSaved = new TModel();
        modelToBeSaved.SaveData(dataToSave);
        var fileName = modelToBeSaved.Id;

        var saveSystem = GetSaveSystem<TModel>(fileName);
        saveSystem.SaveFile(FilePath<TModel>(fileName), modelToBeSaved);
    }

    //Load TModel data from model id and loads into T
    public T LoadData<T, TModel>(T containerToLoadInto)
        where T : class
        where TModel : class, ILoadable<T>, new()
    {
        var modelToBeLoaded = new TModel();
        var data = LoadFile<T, TModel>(modelToBeLoaded.Id);
        return data?.LoadData(containerToLoadInto);
    }

    //Load TModel data from fileName of file and loads into T
    public T LoadData<T, TModel>(T containerToLoadInto, string fileName)
        where T : class
        where TModel : class, ILoadable<T>, new()
    {
        var data = LoadFile<T, TModel>(fileName);
        return data?.LoadData(containerToLoadInto);
    }
    
    //Load TModel data from fileName of file and loads into new T
    public T LoadData<T, TModel>(string fileName)
        where T : class, new()
        where TModel : class, ILoadable<T>, new()
    {
        var containerToLoadInto = new T();
        var data = LoadFile<T, TModel>(fileName);
        return data?.LoadData(containerToLoadInto);
    }
    
    //Load TModel data from file
    public TModel LoadFile<T, TModel>(string fileName)
        where T : class
        where TModel : class, ILoadable<T>, new()
    {
        var saveSystem = GetSaveSystem<TModel>(fileName);
        return saveSystem?.LoadFile(FilePath<TModel>(fileName));
    }

    //Load list of TModels on the folder
    public List<string> LoadFilesNames<TModel>() where TModel : class
    {
        return Directory.GetFiles(FolderPath<TModel>()).ToList();
    }

    //Loads list of TModels from folder
    public List<TModel> LoadModelsList<T, TModel>() 
        where T : class
        where TModel : class, ILoadable<T>, new()  
    {
        var dataList = new List<TModel>();
        
        foreach (var fileName in LoadFilesNames<TModel>())
        {
            var data = LoadFile<T, TModel>(fileName);
            dataList.Add(data);
        }

        return dataList;
    }
    
    //Loads list of new T with TModels from folder
    public List<T> LoadFilesList<T, TModel>() 
        where T : class, new()
        where TModel : class, ILoadable<T>, new()  
    {
        var filesList = new List<T>();
        
        foreach (var fileName in LoadFilesNames<TModel>())
        {
            var loadedFile = LoadData<T, TModel>(fileName);
            filesList.Add(loadedFile);
        }

        return filesList;
    }
    
    //Delete file of type T based on file Id
    public void DeleteData<T>(ILoadable<T> loadable) where T : class
    {
        File.Delete(FilePath<T>(loadable.Id));
    }
    
    //Delete file of type TModel based on file name
    public void DeleteData<TModel>(string fileName) where TModel : class
    {
        File.Delete(FilePath<TModel>(fileName));
    }

    //Open file of type T based on file Id
    public void OpenData<T>(ILoadable<T> loadable) where T : class
    {
        Process.Start(FilePath<T>(loadable.Id));
    }
    
    //Open file of type TModel based on file name
    public void OpenData<TModel>(string fileName) where TModel : class
    {
        Process.Start(FilePath<TModel>(fileName));
    }
    
    //Get filePath of TModel
    private string FilePath<TModel>(string fileName) where TModel : class
    {
        return Path.Combine(FolderPath<TModel>(), $"{fileName}");
    }
    
    //Get folder of TModel
    private string FolderPath<TModel>() where TModel : class
    {
        var fileFolder = typeof(TModel).Name;
        var folderPath = Path.Combine(BaseSavePath, fileFolder);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return folderPath;
    }

    //Get saveSystem from TModel extension
    private SaveSystem<TModel> GetSaveSystem<TModel>(string fileName) where TModel : class
    {
        var path = FilePath<TModel>(fileName);
        var extension = Path.GetExtension(path);

        return extension switch
        {
            ".xml" => new XmlSaveSystem<TModel>(),
            ".json" => new JsonSaveSystem<TModel>(),
            ".bin" => new BinarySaveSystem<TModel>(),
            _ => throw new NotImplementedException()
        };
    }
}