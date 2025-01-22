using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneGraphData : ScriptableObject
{
    [field: SerializeField] public Dictionary<string, SceneNodeData> Nodes { get; set; }

    public void AddSceneNode(DungeonRoomController prefab)
    {
        var id = Guid.NewGuid().ToString();
        var sceneNode = new SceneNodeData(id, prefab);
        Nodes.Add(id, sceneNode);
    }
}
