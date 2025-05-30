using System;
using System.Collections.Generic;

public class BlockedEventNodeData : BaseNodeData
{
    public override bool Open => false;
    private string EventId { get; set; }
    
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

    public override BaseSceneNode ToDomain()
    {
        return new BlockedEventNode(EventId, Name, Id, Doors);
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