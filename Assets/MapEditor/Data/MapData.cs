using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    [field: SerializeField] public List<BaseNodeData> Nodes { get; private set; }

    public T AddNode<T>() where T : BaseNodeData
    {
        Nodes ??= new List<BaseNodeData>();

        var node = CreateInstance<T>();
        Nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        return node;
    }

    public void DeleteNode(BaseNodeData nodeData)
    {
        Nodes ??= new List<BaseNodeData>();
        
        var toRemove = Nodes?.FirstOrDefault(data => data.Id == nodeData.Id);

        if (toRemove != null)
        {
            Nodes.Remove(toRemove);
        }

        AssetDatabase.RemoveObjectFromAsset(toRemove);
        AssetDatabase.SaveAssets();
    }
    
    public Map ToDomain(SceneManager sceneManager)
    {
        var nodes = Nodes.Select(n => n.ToDomain()).ToList();
        return new Map(nodes, sceneManager);
    }
}