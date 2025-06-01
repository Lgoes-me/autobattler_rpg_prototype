using System.Collections.Generic;
using System.Linq;

public class SpawnNodeData : BaseNodeData
{
    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (SpawnNodeDataParams) nodeDataParams;
        
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

    protected override void OnValidate()
    {
        base.OnValidate();
        Id = Name;
    }

    public override BaseSceneNode ToDomain()
    {
        var doors = Doors.Select(d => d.ToDomain(Id)).ToList();
        return new SpawnNode(Id, doors);
    }
}

public class SpawnNodeDataParams : NodeDataParams
{
    public string Id { get; }

    public SpawnNodeDataParams(string id)
    {
        Id = id;
    }
}