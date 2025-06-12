using System.Collections.Generic;

public class SpawnNodeData : BaseNodeData
{
    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (SpawnNodeDataParams) nodeDataParams;
        
        Id = dataParams.Id;
        Name = name = dataParams.Id;

        Doors = new List<DoorData>();

        var spawn = new DoorData
        {
            Name = string.Empty,
            Id = Id
        };

        Doors.Add(spawn);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Id = Name;
    }

    public override BaseSceneNode ToDomain()
    {
        return new SpawnNode(Id, Doors[0].ToDomain(Id));
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