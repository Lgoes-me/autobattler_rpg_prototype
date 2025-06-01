using System;
using System.Collections.Generic;

public class ForkedNodeData : BaseNodeData
{
    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (ForkedNodeDataParams) nodeDataParams;

        Id = dataParams.Id;
        Name = name;

        Doors = new List<DoorData>();

        var door1 = new DoorData
        {
            Name = "in1",
            Id = Guid.NewGuid().ToString()
        };

        var door2 = new DoorData
        {
            Name = "in2",
            Id = Guid.NewGuid().ToString()
        };

        var door3 = new DoorData
        {
            Name = "exit",
            Id = Guid.NewGuid().ToString()
        };

        Doors.Add(door1);
        Doors.Add(door2);
        Doors.Add(door3);
    }

    public override BaseSceneNode ToDomain()
    {
        var in1 = Doors[0].ToDomain(Id);
        var in2 = Doors[1].ToDomain(Id);
        var exit = Doors[2].ToDomain(Id);
        
        return new ForkedNode(in1, in2, exit, Id);
    }
}

public class ForkedNodeDataParams : NodeDataParams
{
    public string Id { get; }

    public ForkedNodeDataParams(string id)
    {
        Id = id;
    }
}