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
        var sceneNode = ScriptableObject.CreateInstance<SceneNodeData>();
        sceneNode.Init(id, prefab);
        Nodes.Add(sceneNode);

        AssetDatabase.AddObjectToAsset(sceneNode, this);
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

        AssetDatabase.RemoveObjectFromAsset(toRemove);
        AssetDatabase.SaveAssets();
    }
}