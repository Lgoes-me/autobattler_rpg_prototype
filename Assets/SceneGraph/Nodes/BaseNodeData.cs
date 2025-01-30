using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNodeData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    public List<SpawnData> Doors { get; protected set; }

    public string Id { get; private set; }
    public Vector2 Position { get; set; }
    public Action OnNodeDataUpdated { get; set; }

    protected void Init(string id, string nodeName)
    {
        Id = id;
        Name = name = nodeName;
        Doors = new List<SpawnData>();
        
        GenerateDoors();
    }

    protected abstract void GenerateDoors();

    private void OnValidate()
    {
        OnNodeDataUpdated?.Invoke();
    }
}