using System.Collections.Generic;

public class GameEventNodeData : BaseNodeData
{
    public void Init(string id)
    {
        Id = id;
        Name = name = string.Empty;

        Doors = new List<SpawnData>();

        var door = new SpawnData
        {
            Name = string.Empty,
            Id = Id
        };

        Doors.Add(door);
    }
}