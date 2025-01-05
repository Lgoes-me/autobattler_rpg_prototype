using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonRoomData : ScriptableObject
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public DungeonRoomController RoomPrefab { get; set; }
    [field: SerializeField] public RoomType RoomType { get; set; }
    [field: SerializeField] public List<SpawnData> Doors { get; set; }
    
    public int NumberOfDoors => RoomPrefab.Doors.Count;

    private void OnValidate()
    {
        var spawns = new SpawnData[NumberOfDoors];

        for (int i = 0; i < NumberOfDoors; i++)
        {
            spawns[i] = new SpawnData
            {
                Id = RoomPrefab.Doors[i].Id,
                SceneDestination = Doors[i].SceneDestination,
                DoorDestination = Doors[i].DoorDestination,
            };
        }

        Doors = new List<SpawnData>(spawns);
    }
}

[Serializable]
public class SpawnData
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public string SceneDestination { get; set; }
    [field: SerializeField] public string DoorDestination { get; set; }
}

public enum RoomType
{
    Entrance,
    Normal,
    Boss
}