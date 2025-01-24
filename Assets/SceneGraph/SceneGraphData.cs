using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneGraphData : ScriptableObject
{
    [field: SerializeReference] [field: SerializeField] public Dictionary<string, SceneNodeData> Nodes { get; set; }

    public SceneNodeData AddSceneNode(DungeonRoomController prefab)
    {
        Nodes ??= new Dictionary<string, SceneNodeData>();
        
        var id = Guid.NewGuid().ToString();
        var sceneNode = new SceneNodeData(id, prefab);
        Nodes.Add(id, sceneNode);
        
        return sceneNode;
    }

    public void DeleteNode(SceneNodeData sceneNodeData)
    {
        Nodes?.Remove(sceneNodeData.Id);
    }
}
