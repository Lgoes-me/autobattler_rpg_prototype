using System.Collections.Generic;

public class SpawnNodeData : BaseNodeData
{
    public void Init(string id)
    {
        Id = id;
        Name = name = id;
        
        Doors = new List<SpawnData>();
        GenerateDoors();
    }

    protected override void GenerateDoors()
    {
        var door = new SpawnData
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
}
