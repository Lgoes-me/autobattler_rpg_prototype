using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneNodeData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    public DungeonRoomController RoomPrefab { get; private set; }
    public List<SpawnData> Doors { get; private set; }
    
    public string Id { get; private set; }
    public Vector2 Position { get; set; }
    public Action OnNodeDataUpdated { get; set; }
    
    public void Init(string id, DungeonRoomController roomPrefab)
    {
        Id = id;
        RoomPrefab = roomPrefab;
        Name = name = RoomPrefab.name;

        var spawns = new SpawnData[RoomPrefab.Doors.Count];

        for (int i = 0; i < RoomPrefab.Doors.Count; i++)
        {
            spawns[i] = new SpawnData
            {
                Name = RoomPrefab.Doors[i].gameObject.name,
                Id = RoomPrefab.Doors[i].Id
            };
        }

        Doors = new List<SpawnData>(spawns);
    }

    private void OnValidate()
    {
        OnNodeDataUpdated?.Invoke();
    }
}