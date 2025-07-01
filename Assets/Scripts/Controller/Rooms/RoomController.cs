using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomController : BaseRoomController
{
    [field:SerializeField] public List<CorridorAreaController> Doors { get; private set; }
    [field:SerializeField] public List<EnemyAreaController> EnemyAreas { get; private set; }
    [field:SerializeField] private BonfireController Bonfire { get; set; }
    [field:SerializeField] private NavMeshSurface Surface { get; set; }
    [field:SerializeField] private Volume PostProcessVolume { get; set; }
    
    private MusicType MusicType { get; set; }
    
    public RoomController Init(SceneNode data)
    {
        Surface.BuildNavMesh();
        
        foreach (var door in Doors)
        {
            var spawn = data.Doors.First(d => d.Start.Id == door.Id);
            door.Init(spawn);
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
        
        return this;
    }
    
    public override void PlayMusic()
    {
        Application.Instance.GetManager<AudioManager>().PlayMusic(MusicType);
    }

    public override void SpawnPlayerAt(string spawn, CinemachineBlendDefinition blend)
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
