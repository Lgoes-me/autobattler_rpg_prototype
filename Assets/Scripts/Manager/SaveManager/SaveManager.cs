using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable MemberCanBePrivate.Global

public class SaveManager
{
    private static string Folder => UnityEngine.Application.persistentDataPath;
    private static string BaseSavePath => Path.Combine(Folder, "Save");

    public SaveManager()
    {
        if (Directory.Exists(BaseSavePath)) 
            return;
        
        Directory.CreateDirectory(BaseSavePath);
    }
    
    //Open folder
#if UNITY_EDITOR
    [MenuItem("GameTools/Save/Open Folder")]
#endif
    public static void OpenFolder()
    {
        Process.Start(BaseSavePath);
    }

    //Clear Directory
#if UNITY_EDITOR
    [MenuItem("GameTools/Save/Clear Folder")]
#endif
    public static void Clear()
    {
        Directory.Delete(BaseSavePath, true);
        Directory.CreateDirectory(BaseSavePath);
    }
    
    //Save T data into file
    public void SaveData<T>(T dataToSave) 
        where T : class, ISavable
    {
        var metadata = dataToSave.Metadata;
        metadata.LastSaved = DateTime.Now;

        var saveSystem = GetSaveSystem<T>(metadata.FileName);
        saveSystem.SaveFile(FilePath<T>(metadata.FileName), dataToSave);
    }

    //Load T data from id
    public T LoadData<T>(T containerToLoadInto)
        where T : class, ISavable
    {
        return LoadData<T>(containerToLoadInto.Metadata.FileName);
    }

    //Load T data from static fileName
    public T LoadData<T>()
        where T : class, ISavable, new()
    {
        var instance = new T();
        return LoadData<T>(instance.Metadata.FileName);
    }
    
    //Load T data from fileName
    public T LoadData<T>(string fileName)
        where T : class, ISavable
    {
        var saveSystem = GetSaveSystem<T>(fileName);
        return saveSystem?.LoadFile(FilePath<T>(fileName));
    }

    //Load list of T names from the folder
    public List<string> LoadFilesNames<TModel>() where TModel : class
    {
        return Directory.GetFiles(FolderPath<TModel>()).ToList();
    }

    //Loads list of T from folder
    public List<T> LoadList<T>()
        where T : class, ISavable
    {
        var dataList = new List<T>();
        
        foreach (var fileName in LoadFilesNames<T>())
        {
            var data = LoadData<T>(fileName);
            dataList.Add(data);
        }

        return dataList;
    }
    
    //Delete file of type T based on file Id
    public void DeleteData<T>(ISavable loadable) 
        where T : class
    {
        File.Delete(FilePath<T>(loadable.Metadata.FileName));
    }
    
    //Delete file of type T based on file name
    public void DeleteData<T>(string fileName)
        where T : class
    {
        File.Delete(FilePath<T>(fileName));
    }

    //Open file of type T based on file Id
    public void OpenData<T>(ISavable loadable)
        where T : class
    {
        Process.Start(FilePath<T>(loadable.Metadata.FileName));
    }
    
    //Open file of type T based on file name
    public void OpenData<T>(string fileName) 
        where T : class
    {
        Process.Start(FilePath<T>(fileName));
    }
    
    //Get filePath of T
    private string FilePath<T>(string fileName) 
        where T : class
    {
        return Path.Combine(FolderPath<T>(), $"{fileName}");
    }
    
    //Get folder of T
    private string FolderPath<T>() 
        where T : class
    {
        var fileFolder = typeof(T).Name;
        var folderPath = Path.Combine(BaseSavePath, fileFolder);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return folderPath;
    }

    //Get saveSystem from T extension
    private SaveSystem<T> GetSaveSystem<T>(string fileName) 
        where T : class
    {
        var path = FilePath<T>(fileName);
        var extension = Path.GetExtension(path);

        return extension switch
        {
            ".xml" => new XmlSaveSystem<T>(),
            ".json" => new JsonSaveSystem<T>(),
            ".bin" => new BinarySaveSystem<T>(),
            _ => throw new NotImplementedException()
        };
    }
}