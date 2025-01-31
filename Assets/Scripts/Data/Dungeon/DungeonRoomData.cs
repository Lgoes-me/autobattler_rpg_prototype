using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class DungeonRoomData : ScriptableObject
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public DungeonRoomController RoomPrefab { get; set; }
    [field: SerializeField] public RoomType RoomType { get; set; }
    [field: SerializeField] public List<SpawnData> Doors { get; set; }
    public int NumberOfDoors => RoomPrefab.Doors.Count;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

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
#endif
}

public enum RoomType
{
    Entrance,
    Normal,
    Boss
}