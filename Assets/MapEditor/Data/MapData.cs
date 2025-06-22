using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    [field: SerializeField] public List<BaseNodeData> Nodes { get; private set; }

#if UNITY_EDITOR

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

#endif

    public Map ToDomain()
    {
        var nodes = Nodes.Select(n => n.ToDomain()).ToList();
        return new Map(nodes);
    }
}