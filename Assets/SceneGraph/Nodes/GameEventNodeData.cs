using System.Collections.Generic;
using UnityEngine;

public class GameEventNodeData : BaseNodeData
{
    [field: SerializeField] private GameAction GameAction { get; set; }
    
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

    public override BaseSceneNode ToDomain()
    {
        return new GameEventNode(Name, Id, Doors, GameAction);
    }
}