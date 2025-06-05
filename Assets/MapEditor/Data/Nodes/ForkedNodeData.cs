using System;
using System.Collections.Generic;

public class ForkedNodeData : BaseNodeData
{
    private DoorData In1 { get; set; }
    private DoorData In2 { get; set; }
    private DoorData Exit { get; set; }
    
    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (ForkedNodeDataParams) nodeDataParams;

        Id = dataParams.Id;
        Name = name;

        Doors = new List<DoorData>();

        In1 = new DoorData
        {
            Name = "in1",
            Id = Guid.NewGuid().ToString()
        };

        In1 = new DoorData
        {
            Name = "in2",
            Id = Guid.NewGuid().ToString()
        };

        Exit = new DoorData
        {
            Name = "exit",
            Id = Guid.NewGuid().ToString()
        };

        Doors.Add(In1);
        Doors.Add(In2);
        Doors.Add(Exit);
    }

    public override BaseSceneNode ToDomain()
    {
        return new ForkedNode(In1.ToDomain(Id), In2.ToDomain(Id), Exit.ToDomain(Id), Id);
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