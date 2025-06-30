using System;
using System.Collections.Generic;

public class ForkedNodeData : BaseNodeData
{
    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (ForkedNodeDataParams) nodeDataParams;

        Id = dataParams.Id;
        Name = name = Id;

        Doors = new List<DoorData>();

        var in1 = new DoorData
        {
            Name = "in1",
            Id = Guid.NewGuid().ToString()
        };

        var in2 = new DoorData
        {
            Name = "in2",
            Id = Guid.NewGuid().ToString()
        };

        var exit = new DoorData
        {
            Name = "exit",
            Id = Guid.NewGuid().ToString()
        };

        Doors.Add(in1);
        Doors.Add(in2);
        Doors.Add(exit);
    }

    public override BaseSceneNode ToDomain()
    {
        return new ForkedNode(Doors[0].ToDomain(Id), Doors[1].ToDomain(Id), Doors[2].ToDomain(Id), Id);
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