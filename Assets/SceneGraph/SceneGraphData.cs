using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class SceneGraphData : ScriptableObject
{
    [field: SerializeField] public List<SceneNodeData> Nodes { get; set; }

    private void OnEnable()
    {
        Nodes ??= new List<SceneNodeData>();
    }

    public SceneNodeData AddSceneNode(DungeonRoomController prefab)
    {
        Nodes ??= new List<SceneNodeData>();


        var id = Guid.NewGuid().ToString();
        var sceneNode = new SceneNodeData(id, prefab);
        Nodes.Add(sceneNode);

        AssetDatabase.SaveAssets();

        return sceneNode;
    }

    public void DeleteNode(SceneNodeData sceneNodeData)
    {
        var toRemove = Nodes?.FirstOrDefault(data => data.Id == sceneNodeData.Id);

        if (toRemove != null)
        {
            Nodes.Remove(toRemove);
        }

        AssetDatabase.SaveAssets();
    }
}