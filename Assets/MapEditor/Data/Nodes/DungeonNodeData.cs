using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonNodeData : BaseNodeData
{
    [field: SerializeField] private List<DungeonRoomData> AvailableRooms { get; set; }

    [field: SerializeField] private int MaximumDoors { get; set; }
    [field: SerializeField] private int MinimumDeepness { get; set; }
    [field: SerializeField] private int MaximumDeepness { get; set; }

    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (DungeonNodeDataParams) nodeDataParams;
        
        Id = dataParams.Id;
        Name = name = dataParams.Id;

        Doors = new List<DoorData>();

        var door = new DoorData
        {
            Name = string.Empty,
            Id = Id
        };

        Doors.Add(door);
    }
    
    public override BaseSceneNode ToDomain()
    {
        var availableRooms = AvailableRooms.Select(r => r.ToDomain()).ToList();
        var doors = Doors.Select(d => d.ToDomain(Id)).ToList();
        return new DungeonNode(Name, Id, doors, availableRooms, MaximumDoors, MinimumDeepness, MaximumDeepness);
    }
}

public class DungeonNodeDataParams : NodeDataParams
{
    public string Id { get; }

    public DungeonNodeDataParams(string id)
    {
        Id = id;
    }
}

[Serializable]
public class DungeonRoomData
{
    [field: SerializeField] public RoomController RoomPrefab { get; set; }
    [field: SerializeField] public RoomType RoomType { get; set; }

    public DungeonRoomNode ToDomain()
    {
        var doors = new List<DoorData>();

        foreach (var t in RoomPrefab.Doors)
        {
            var spawn = new DoorData
            {
                Name = t.gameObject.name,
                Id = t.Id
            };

            doors.Add(spawn);
        }

        return new DungeonRoomNode(
            RoomPrefab.gameObject.name, 
            string.Empty, 
            doors.Select(d => d.ToDomain("")).ToList(),
            RoomPrefab,
            RoomType);
    }
}

public enum RoomType
{
    Unknown,
    Entrance,
    Normal,
    Boss
}