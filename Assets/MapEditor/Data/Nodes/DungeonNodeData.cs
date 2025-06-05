using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonNodeData : BaseNodeData
{
    private DoorData Entrance { get; set; }
    private DoorData Exit { get; set; }
    [field: SerializeField] private List<DungeonRoomData> AvailableRooms { get; set; }

    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (DungeonNodeDataParams) nodeDataParams;
        
        Id = dataParams.Id;
        Name = name = dataParams.Id;

        Doors = new List<DoorData>();

        Entrance = new DoorData
        {
            Name = "entrance",
            Id = Guid.NewGuid().ToString()
        };
        
        Exit = new DoorData
        {
            Name = "exit",
            Id = Guid.NewGuid().ToString()
        };

        Doors.Add(Entrance);
        Doors.Add(Exit);
    }
    
    public override BaseSceneNode ToDomain()
    {
        return new DungeonNode(Id, Entrance.ToDomain(Id), Exit.ToDomain(Id), AvailableRooms);
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

    public DungeonRoom ToDomain(string dungeonId)
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

        return new DungeonRoom(
            string.Empty, 
            doors.Select(d => d.ToDomain(dungeonId)).ToList(),
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