using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseNodeData : ScriptableObject
{
    [field: SerializeField] public string Name { get; protected set; }

    [field: HideInInspector] [field: SerializeField] public string Id { get; protected set; }
    [field: HideInInspector] [field: SerializeField] public List<SpawnData> Doors { get; protected set; }
    [field: HideInInspector] [field: SerializeField] public Vector2 Position { get; protected set; }

    public Action OnNodeDataUpdated { get; set; }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }

    protected virtual void OnValidate()
    {
        OnNodeDataUpdated?.Invoke();
    }
}

[Serializable]
public class SpawnData
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Port { get; set; }
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public string SceneDestination { get; set; }
    [field: SerializeField] public string DoorDestination { get; set; }
    [field: SerializeField] public bool SetUp { get; set; }

    public SpawnDomain ToDomain()
    {
        return new SpawnDomain(DoorDestination, SceneDestination);
    }
}