using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneNodeData  
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public Vector2 Position { get; set; }
    [field: SerializeField] public DungeonRoomController RoomPrefab { get; set; }
    [field: SerializeField] public List<SpawnData> Doors { get; set; }
    
    public SceneNodeData(string id, DungeonRoomController roomPrefab)
    {
        Id = id;
        RoomPrefab = roomPrefab;

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
}