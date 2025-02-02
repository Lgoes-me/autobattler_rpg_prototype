using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class SceneGraphData : ScriptableObject
{
    [field: SerializeField] public List<BaseNodeData> Nodes { get; set; }

    public Dictionary<string, BaseNodeData> AllNodesById { get; set; }
    public Dictionary<string, SceneNodeData> SceneNodeById { get; set; }
    public Dictionary<string, SpawnNodeData> SpawnsByName { get; set; }

    public void Init()
    {
        AllNodesById = Nodes.ToDictionary(n => n.Id, n => n);

        SceneNodeById = Nodes
            .Where(n => n is SceneNodeData)
            .ToDictionary(n => n.Id, n => n as SceneNodeData);

        SpawnsByName = Nodes
            .Where(n => n is SpawnNodeData)
            .ToDictionary(n => n.Name, n => n as SpawnNodeData);
    }

    public SceneNodeData AddSceneNode(RoomController prefab)
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
        sceneNode.Init(id);
        Nodes.Add(sceneNode);

        AssetDatabase.AddObjectToAsset(sceneNode, this);
        AssetDatabase.SaveAssets();

        return sceneNode;
    }

    public DungeonNodeData AddDungeonNode()
    {
        Nodes ??= new List<BaseNodeData>();

        var id = Guid.NewGuid().ToString();
        var sceneNode = CreateInstance<DungeonNodeData>();
        sceneNode.Init(id);
        Nodes.Add(sceneNode);

        AssetDatabase.AddObjectToAsset(sceneNode, this);
        AssetDatabase.SaveAssets();

        return sceneNode;
    }

    public GameEventNodeData AddGameEventNode()
    {
        Nodes ??= new List<BaseNodeData>();

        var id = Guid.NewGuid().ToString();
        var sceneNode = CreateInstance<GameEventNodeData>();
        sceneNode.Init(id);
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