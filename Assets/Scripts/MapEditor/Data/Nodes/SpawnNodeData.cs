using System.Collections.Generic;

public class SpawnNodeData : BaseNodeData
{
    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (SpawnNodeDataParams) nodeDataParams;
        
        Id = dataParams.Id;
        Name = name = dataParams.Id;

        Doors = new List<DoorData>
        {
            new()
            {
                Name = string.Empty,
                Id = Id
            }
        };
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        name = Id = Name;
    }

    public override BaseNode ToDomain()
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