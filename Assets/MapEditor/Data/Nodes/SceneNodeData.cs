using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneNodeData : BaseNodeData
{
    [field: SerializeField] public RoomController RoomPrefab { get; private set; }
    [field: SerializeField] public List<CombatEncounterData> CombatEncounters { get; protected set; }
    
    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (SceneNodeDataParams) nodeDataParams;

        Id = dataParams.Id;
        Name = name = dataParams.RoomPrefab.name;
        RoomPrefab = dataParams.RoomPrefab;
        Doors = new List<DoorData>();

        foreach (var spawn in RoomPrefab.Doors)
        {
            var door = new DoorData
            {
                Name = spawn.gameObject.name,
                Id = spawn.Id
            };

            Doors.Add(door);
        }
    }
    
    public override BaseSceneNode ToDomain()
    {
        return new SceneNode(Name, Id, Doors, RoomPrefab);
    }
}

public class SceneNodeDataParams : NodeDataParams
{
    public string Id { get; }
    public RoomController RoomPrefab { get; }

    public SceneNodeDataParams(string id, RoomController roomPrefab)
    {
        Id = id;
        RoomPrefab = roomPrefab;
    }
}

[Serializable]
public class CombatEncounterData
{
    [field: SerializeField] public List<EnemyData> Enemies { get; protected set; }

    /*public SpawnDomain ToDomain()
    {
        return new SpawnDomain(DoorDestination, SceneDestination);
    }*/
}
