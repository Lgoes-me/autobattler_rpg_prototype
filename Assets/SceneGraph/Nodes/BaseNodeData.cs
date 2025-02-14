using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNodeData : ScriptableObject
{
    [field: SerializeField] public string Name { get; protected set; }

    [field: HideInInspector] [field: SerializeField] public string Id { get; protected set; }
    [field: HideInInspector] [field: SerializeField] public List<SpawnData> Doors { get; protected set; }
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
}