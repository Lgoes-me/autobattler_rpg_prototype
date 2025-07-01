using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomController : BaseRoomController<SceneNode>
{
    [field:SerializeField] public List<CorridorAreaController> Doors { get; private set; }
    [field:SerializeField] public List<EnemyAreaController> EnemyAreas { get; private set; }
    [field:SerializeField] private BonfireController Bonfire { get; set; }
    [field:SerializeField] private NavMeshSurface Surface { get; set; }
    [field:SerializeField] private Volume PostProcessVolume { get; set; }
    
    private MusicType MusicType { get; set; }
    
    protected override BaseRoomController<SceneNode> InternalInit(SceneNode data, Spawn spawn, CinemachineBlendDefinition blend)
    {
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

        MusicType = data.Music;
        PostProcessVolume.profile = data.PostProcessProfile;
        
        SpawnPlayerAt(spawn.Id, blend);
        
        Application.Instance.GetManager<GameSaveManager>().SetSpawn(spawn);
        Application.Instance.GetManager<PartyManager>().SetPartyToFollow(true);
        Application.Instance.GetManager<InterfaceManager>().ShowBattleCanvas();
        Application.Instance.GetManager<AudioManager>().PlayMusic(MusicType);
        
        return this;
    }

    private void SpawnPlayerAt(string spawn, CinemachineBlendDefinition blend)
    {
        var door = Doors.FirstOrDefault(d => d.Id == spawn);
        
        if (door != null)
        {
            door.SpawnPlayer(blend);
        }
        else if (Bonfire != null && Bonfire.Spawn.Id == spawn)
        {
            Bonfire.Spawn.SpawnPlayer(blend);
        }
    }
}
