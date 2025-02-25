using System.Collections.Generic;

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
        return new SpawnNode(Name, Id, Doors);
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