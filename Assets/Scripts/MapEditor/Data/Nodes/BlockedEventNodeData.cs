using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockedEventNodeData : BaseNodeData
{
    private string EventId { get; set; }
    [field: SerializeField] private DialogueData Dialogue { get; set; }

    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (BlockedEventNodeDataParams) nodeDataParams;

        Id = dataParams.Id;
        Name = name;

        Doors = new List<DoorData>();

        var door1 = new DoorData
        {
            Name = string.Empty,
            Id = Guid.NewGuid().ToString()
        };

        var door2 = new DoorData
        {
            Name = string.Empty,
            Id = Guid.NewGuid().ToString()
        };

        Doors.Add(door1);
        Doors.Add(door2);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        name = Name;
        EventId = Name;
    }

    public override BaseNode ToDomain()
    {
        var doors = Doors.Select(d => d.ToDomain(Id)).ToList();
        return new BlockedEventNode(EventId, Dialogue, Id, doors);
    }
}

public class BlockedEventNodeDataParams : NodeDataParams
{
    public string Id { get; }

    public BlockedEventNodeDataParams(string id)
    {
        Id = id;
    }
}