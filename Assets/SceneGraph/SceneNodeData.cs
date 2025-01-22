using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneNodeData
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public DungeonRoomController RoomPrefab { get; set; }
    
    [field: SerializeField] public List<SpawnData> Doors { get; set; }
    
    public int NumberOfDoors => RoomPrefab.Doors.Count;

    public SceneNodeData(string id)
    {
        Id = id;
    }
    
    public SceneNodeData(string id, DungeonRoomController roomPrefab)
    {
        Id = id;
        RoomPrefab = roomPrefab;

        var spawns = new SpawnData[NumberOfDoors];

        for (int i = 0; i < NumberOfDoors; i++)
        {
            spawns[i] = new SpawnData
            {
                Id = RoomPrefab.Doors[i].Id
            };
        }

        Doors = new List<SpawnData>(spawns);
    }
}