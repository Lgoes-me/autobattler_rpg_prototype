using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNodeData : ScriptableObject
{
    [field: SerializeField] public string Name { get; protected set; }

    public string Id { get; protected set; }
    public List<SpawnData> Doors { get; protected set; }
    public Vector2 Position { get; set; }
    public Action OnNodeDataUpdated { get; set; }

    protected abstract void GenerateDoors();

    protected virtual void OnValidate()
    {
        OnNodeDataUpdated?.Invoke();
    }
}