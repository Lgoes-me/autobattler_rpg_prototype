using System;
using UnityEngine;

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