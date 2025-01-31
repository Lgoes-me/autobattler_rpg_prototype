using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseNodeData : ScriptableObject
{
    [field: SerializeField] public string Name { get; protected set; }

    [field: SerializeField] public string Id { get; protected set; }
    [field: SerializeField] public List<SpawnData> Doors { get; protected set; }
    public Vector2 Position { get; set; }
    public Action OnNodeDataUpdated { get; set; }

    protected virtual void OnValidate()
    {
        OnNodeDataUpdated?.Invoke();
    }
}