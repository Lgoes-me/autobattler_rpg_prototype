using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [field:SerializeField] public List<CorridorAreaController> Doors { get; private set; }
    [field:SerializeField] private List<EnemyAreaController> EnemyAreas { get; set; }
    [field:SerializeField] private BonfireController Bonfire { get; set; }
    [field:SerializeField] private NavMeshSurface Surface { get; set; }
    [field:SerializeField] public Camera PreviewCamera { get; private set; }
    
    public MusicType MusicType { get; private set; }
    
    public RoomController Init(SceneNode sceneData)
    {
        foreach (var door in Doors)
        {
            var doorSpawnData = sceneData.Doors.First(d => d.Id == door.Id);
            door.Spawn = doorSpawnData.ToDomain();
        }

        if (Bonfire != null)
        {
            Bonfire.Init(sceneData.Id);
        }

        foreach (var enemyArea in EnemyAreas)
        {
            enemyArea.Init(sceneData.Id);
        }
        
        Surface.BuildNavMesh();
        
        Destroy(PreviewCamera.gameObject);

        MusicType = sceneData.Music;
        
        return this;
    }

    public void PlayMusic()
    {
        Application.Instance.AudioManager.PlayMusic(MusicType);
    }
    
    public void SpawnPlayerAt(string spawn)
    {
        var door = Doors.FirstOrDefault(d => d.Id == spawn);
        
        if (door != null)
        {
            Application.Instance.PlayerManager.SpawnPlayerAt(door);
            return;
        }

        if (Bonfire != null && Bonfire.Spawn.Id == spawn)
        {
            Application.Instance.PlayerManager.SpawnPlayerAt(Bonfire.Spawn);
            return;
        }
    }
}
