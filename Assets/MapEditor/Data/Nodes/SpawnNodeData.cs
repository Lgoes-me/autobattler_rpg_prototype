using System.Collections.Generic;
using System.Linq;

public class SpawnNodeData : BaseNodeData
{
    private DoorData Spawn { get; set; }
    
    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (SpawnNodeDataParams) nodeDataParams;
        
        Id = dataParams.Id;
        Name = name = dataParams.Id;

        Doors = new List<DoorData>();

        Spawn = new DoorData
        {
            Name = string.Empty,
            Id = Id
        };

        Doors.Add(Spawn);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Id = Name;
    }

    public override BaseSceneNode ToDomain()
    {
        return new SpawnNode(Id, Spawn.ToDomain(Id));
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