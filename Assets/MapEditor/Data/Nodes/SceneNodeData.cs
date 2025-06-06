using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class SceneNodeData : BaseNodeData
{
    [field: SerializeField] public RoomController RoomPrefab { get; private set; }
    [field: SerializeField] private List<CombatEncounterData> CombatEncounters { get; set; }
    [field: SerializeField] private VolumeProfile PostProcessProfile { get; set; }

    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (SceneNodeDataParams) nodeDataParams;

        Id = dataParams.Id;
        Name = name = dataParams.RoomPrefab.name;
        RoomPrefab = dataParams.RoomPrefab;
        Doors = new List<DoorData>();
        CombatEncounters = new List<CombatEncounterData>();

        foreach (var corridorController in RoomPrefab.Doors)
        {
            var door = new DoorData
            {
                Name = corridorController.gameObject.name,
                Id = corridorController.Id,
                Direction = corridorController.Direction,
                Theme = corridorController.Theme
            };

            Doors.Add(door);
        }

        foreach (var enemyArea in RoomPrefab.EnemyAreas)
        {
            var enemyDataList = new List<EnemyData>();
            
            foreach (var enemyController in enemyArea.EnemyControllers)
            {
                var enemyData = new EnemyData()
                {
                    name = enemyController.name,
                };

                enemyDataList.Add(enemyData);
            }
            var combatEncounter = new CombatEncounterData()
            {
                Enemies = enemyDataList,
            };
            
            CombatEncounters.Add(combatEncounter);
        }
    }

    public override BaseSceneNode ToDomain()
    {
        var doors = Doors.Select(d => d.ToDomain(Id)).ToList();
        return new SceneNode(Id, doors, RoomPrefab, CombatEncounters, PostProcessProfile);
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
    [field: SerializeField] public List<EnemyData> Enemies { get; internal set; }
    [field: SerializeReference] [field: SerializeField] public GameAction OnVictory { get; private set; }
    [field: SerializeReference] [field: SerializeField] public GameAction OnDefeat { get; private set; }
}

[Serializable]
public class EnemyData
{
    [field: HideInInspector] public string name;
    [field: SerializeField] public bool IsBoss { get; set; }
    [field: SerializeField] public EnemyInfo EnemyInfo { get; set; }
    [field: SerializeField] private string Animation { get; set; }
    [field: SerializeField] private bool Forward { get; set; }
    public PawnController PawnController { get; set; }
    
    public void PreparePawn(PawnController pawnController)
    {
        PawnController = pawnController;
        PawnController.Init(EnemyInfo.ToDomain(TeamType.Enemies));
        PawnController.CharacterController.SetDirection((Forward ? 1 : -1) * PawnController.transform.forward);
        PawnController.CharacterController.SetAnimationState(new DefaultState(Animation, () => PawnController.CharacterController.SetAnimationState(new IdleState())));
    }
}

[Serializable]
public class EnemyInfo
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private WeaponType WeaponType { get; set; }
    [field: SerializeField] private CharacterController Mount { get; set; }
    [field: SerializeField] private int Level { get; set; } = 1;

    public Pawn ToDomain(TeamType teamType)
    {
        return PawnData.ToDomain(PawnStatus.Enemy, teamType, Level, WeaponType, new MountComponent(Mount));
    }
}