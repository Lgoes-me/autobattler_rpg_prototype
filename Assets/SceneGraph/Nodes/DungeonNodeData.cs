using System.Collections.Generic;

public class DungeonNodeData : BaseNodeData
{
    public void Init(string id)
    {
        Id = id;
        Name = name = id;

        Doors = new List<SpawnData>();

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