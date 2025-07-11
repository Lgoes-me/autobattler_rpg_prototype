using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SceneNodeData : BaseNodeData
{
    [field: HideInInspector] [field: SerializeField] public RoomController RoomPrefab { get; private set; }
    [field: SerializeField] private List<CombatEncounterData> CombatEncounters { get; set; }
    [field: SerializeField] private VolumeProfile PostProcessProfile { get; set; }
    [field: SerializeField] private MusicType Music { get; set; }

    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (SceneNodeDataParams) nodeDataParams;

        Id = dataParams.Id;

        Name = name = dataParams.RoomPrefab.name;
        RoomPrefab = dataParams.RoomPrefab;

        Doors = RoomPrefab.GetDoorDataList;
        CombatEncounters = RoomPrefab.GetCombatEncountersDataList;
    }

    [ContextMenu("Update")]
    public void Update()
    {
        var newCombatEncounters = RoomPrefab.GetCombatEncountersDataList;

        foreach (var newCombatEncounter in newCombatEncounters)
        {
            var combatEncounter = CombatEncounters.FirstOrDefault(c => c.Id == newCombatEncounter.Id);

            if (combatEncounter == null)
                continue;

            for (var index = 0; index < newCombatEncounter.Enemies.Count; index++)
            {
                var newEnemy = newCombatEncounter.Enemies[index];
                
                var enemy = combatEncounter.Enemies.FirstOrDefault(e => e.name == newEnemy.name);

                if (enemy == null)
                    continue;

                newCombatEncounter.Enemies[index] = enemy;
            }
        }

        CombatEncounters = newCombatEncounters;
        
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);        
#endif
    }

    public override BaseNode ToDomain()
    {
        var doors = Doors.Select(d => d.ToDomain(Id)).ToList();

        return new SceneNode(
            Id,
            doors,
            RoomPrefab,
            CombatEncounters,
            PostProcessProfile,
            Music);
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
    [field: HideInInspector] public string Id { get; internal set; }
    [field: SerializeField] public int Experience { get; internal set; }
    [field: SerializeField] public List<EnemyData> Enemies { get; internal set; }

    [field: SerializeReference] [field: SerializeField] public GameAction OnVictory { get; private set; }
    [field: SerializeReference] [field: SerializeField] public GameAction OnDefeat { get; private set; }
}