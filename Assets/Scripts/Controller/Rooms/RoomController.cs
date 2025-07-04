using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.AI.Navigation;
using UnityEngine;

public class RoomController : BaseRoomController<SceneNode>
{
    [field:SerializeField] private List<CorridorAreaController> Doors { get; set; }
    [field:SerializeField] private List<EnemyAreaController> EnemyAreas { get; set; }
    [field:SerializeField] private BonfireController Bonfire { get; set; }
    [field:SerializeField] private NavMeshSurface Surface { get; set; }

    protected override BaseRoomController<SceneNode> InternalInit(SceneNode data, Spawn spawn, CinemachineBlendDefinition blend)
    {
        base.InternalInit(data, spawn, blend);
        
        Surface.BuildNavMesh();
        
        foreach (var door in Doors)
        {
            door.Init(data.Doors.First(d => d.Start.Id == door.Id));
        }

        if (Bonfire != null)
        {
            Bonfire.Init(data.Id);
        }

        for (var index = 0; index < EnemyAreas.Count; index++)
        {
            var enemyArea = EnemyAreas[index];
            enemyArea.Init(data.Id, data.CombatEncounters[index]);
        }

        var doorToSpawn = Doors.FirstOrDefault(d => d.Id == spawn.Id);
        
        if (doorToSpawn != null)
        {
            doorToSpawn.SpawnPlayer(blend);
        }
        else if (Bonfire != null && Bonfire.Spawn.Id == spawn.Id)
        {
            Bonfire.Spawn.SpawnPlayer(blend);
        }

        Application.Instance.GetManager<PartyManager>().SetPartyToFollow(true);
        Application.Instance.GetManager<InterfaceManager>().ShowBattleCanvas();
        
        return this;
    }

    public List<DoorData> GetDoorDatas =>  Doors.Select(d => d.ToDoorData()).ToList();
    public List<CombatEncounterData> GetCombatEncountersDatas => EnemyAreas.Select(e => e.ToCombatEncounterData()).ToList();
    
}
