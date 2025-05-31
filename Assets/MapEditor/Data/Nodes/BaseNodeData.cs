using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNodeData : ScriptableObject
{
    [field: SerializeField] public string Name { get; protected set; }
    
    [field: HideInInspector] [field: SerializeField] public string Id { get; protected set; }
    [field: HideInInspector] [field: SerializeField] public List<DoorData> Doors { get; protected set; }
    [field: HideInInspector] [field: SerializeField] public Vector2 Position { get; private set; }
    
    public Action OnNodeDataUpdated { get; set; }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }

    protected virtual void OnValidate()
    {
        OnNodeDataUpdated?.Invoke();
    }
    
    public abstract BaseSceneNode ToDomain();
    public abstract void Init(NodeDataParams nodeDataParams);
}

public class NodeDataParams
{
    
}

[Serializable]
public class DoorData
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Port { get; set; }
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public string SceneDestination { get; set; }
    [field: SerializeField] public string DoorDestination { get; set; }
    [field: SerializeField] public bool SetUp { get; set; }

    public void Clear()
    {
        SceneDestination = "";
        DoorDestination = "";
        Port = "";      
        SetUp = false;
    }

    public void Connect(string sceneDestination, string doorDestination, string port = "")
    {
        SceneDestination = sceneDestination;
        DoorDestination = doorDestination;
        Port = port;
        SetUp = true;
    }
    
    public SpawnDomain ToDomain(string nodeId)
    {
        return new SpawnDomain(Id, nodeId, DoorDestination, SceneDestination);
    }
}