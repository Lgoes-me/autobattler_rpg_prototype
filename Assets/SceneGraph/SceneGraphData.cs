using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneGraphData : ScriptableObject
{
    [field: SerializeField] public Dictionary<string, SceneNodeData> Nodes { get; set; }

    public void AddSceneNode(DungeonRoomController prefab)
    {
        Nodes ??= new Dictionary<string, SceneNodeData>();
        
        var id = Guid.NewGuid().ToString();
        var sceneNode = new SceneNodeData(id, prefab);
        Nodes.Add(id, sceneNode);
    }

    public SceneNodeData CreateSceneNode()
    {
        Nodes ??= new Dictionary<string, SceneNodeData>();
        
        var id = Guid.NewGuid().ToString();
        var sceneNode = new SceneNodeData(id);
        Nodes.Add(id, sceneNode);

        return sceneNode;
    }
}
