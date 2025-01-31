using System.Collections.Generic;
using UnityEngine;

public class SceneNodeData : BaseNodeData
{
    [field: SerializeField] public DungeonRoomController RoomPrefab { get; private set; }
    
    public void Init(string id, DungeonRoomController roomPrefab)
    {
        Id = id;
        Name = name = roomPrefab.name;
        RoomPrefab = roomPrefab;
        Doors = new List<SpawnData>();
        
        foreach (var spawn in RoomPrefab.Doors)
        {
            var door = new SpawnData
            {
                Name = spawn.gameObject.name,
                Id = spawn.Id
            };

            Doors.Add(door);
        } 
    }
}