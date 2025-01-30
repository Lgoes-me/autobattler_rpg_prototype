using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class SceneGraphData : ScriptableObject
{
    [field: SerializeField] public List<BaseNodeData> Nodes { get; set; }

    private void OnEnable()
    {
        Nodes ??= new List<BaseNodeData>();
    }

    public SceneNodeData AddSceneNode(DungeonRoomController prefab)
    {
        Nodes ??= new List<BaseNodeData>();

        var id = Guid.NewGuid().ToString();
        var sceneNode = CreateInstance<SceneNodeData>();
        sceneNode.Init(id, prefab);
        Nodes.Add(sceneNode);

        AssetDatabase.AddObjectToAsset(sceneNode, this);
        AssetDatabase.SaveAssets();

        return sceneNode;
    }

    public SpawnNodeData AddSpawnNode()
    {
        Nodes ??= new List<BaseNodeData>();

        var id = Guid.NewGuid().ToString();
        var sceneNode = CreateInstance<SpawnNodeData>();
        sceneNode.Init(id, id);
        Nodes.Add(sceneNode);

        AssetDatabase.AddObjectToAsset(sceneNode, this);
        AssetDatabase.SaveAssets();

        return sceneNode;
    }
    
    public void DeleteNode(BaseNodeData nodeData)
    {
        var toRemove = Nodes?.FirstOrDefault(data => data.Id == nodeData.Id);

        if (toRemove != null)
        {
            Nodes.Remove(toRemove);
        }

        AssetDatabase.RemoveObjectFromAsset(toRemove);
        AssetDatabase.SaveAssets();
    }
}