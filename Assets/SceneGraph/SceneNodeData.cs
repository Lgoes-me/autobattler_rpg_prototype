using System.Collections.Generic;
using UnityEngine;

public class SceneNodeData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public DungeonRoomController RoomPrefab { get; private set; }
    [field: SerializeField] public List<SpawnData> Doors { get; private set; }
    
    public string Id { get; private set; }
    public Vector2 Position { get; set; }

    public void Init(string id, DungeonRoomController roomPrefab)
    {
        Name = name = RoomPrefab?.gameObject.name ?? "Scene";
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