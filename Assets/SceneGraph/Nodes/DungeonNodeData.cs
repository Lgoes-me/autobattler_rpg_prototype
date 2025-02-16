using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DungeonNodeData : BaseNodeData
{
    [field: SerializeField] private List<DungeonRoomData> AvailableRooms { get; set; }

    [field: SerializeField] private int MaximumDoors { get; set; }
    [field: SerializeField] private int MinimumDeepness { get; set; }
    [field: SerializeField] private int MaximumDeepness { get; set; }

    public void Init(string id)
    {
        Id = id;
        Name = name = id;

        Doors = new List<SpawnData>();

        var door = new SpawnData
        {
            Name = string.Empty,
            Id = Id
        };

        Doors.Add(door);
    }

    public override BaseSceneNode ToDomain()
    {
        var availableRooms = AvailableRooms.Select(r => r.ToDomain()).ToList();
        return new DungeonNode(Name, Id, Doors, availableRooms, MaximumDoors, MinimumDeepness, MaximumDeepness);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Id = Name;

#if UNITY_EDITOR
        foreach (var availableRoom in AvailableRooms)
        {
            availableRoom.OnValidate();
        }
#endif
    }
}

[Serializable]
public class DungeonRoomData
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public RoomController RoomPrefab { get; set; }
    [field: SerializeField] public RoomType RoomType { get; set; }
    [field: SerializeField] public List<SpawnData> Doors { get; set; }

    public int NumberOfDoors => RoomPrefab.Doors.Count;

    public DungeonSceneNode ToDomain()
    {
        return new DungeonSceneNode(Id, Id, Doors, RoomPrefab, RoomType);
    }
    
#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        if (string.IsNullOrWhiteSpace(Id))
        {
            Id = Guid.NewGuid().ToString();
        }

        if(RoomPrefab == null)
        {
            return;
        }
        
        Doors = new List<SpawnData>();

        for (int i = 0; i < RoomPrefab.Doors.Count; i++)
        {
            var spawn = new SpawnData
            {
                Id = RoomPrefab.Doors[i].Id,
                SceneDestination = Doors[i].SceneDestination,
                DoorDestination = Doors[i].DoorDestination,
            };

            Doors.Add(spawn);
        }
    }
#endif
}

public enum RoomType
{
    Entrance,
    Normal,
    Boss
}