using System;
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
    
    [field:SerializeField] private LightSource LightSource { get; set; }
    [field:SerializeField] private List<Material> Materials { get; set; }
    
    private MusicType MusicType { get; set; }
    
    public RoomController Init(SceneNode sceneData)
    {
        Destroy(PreviewCamera.gameObject);
        Surface.BuildNavMesh();
        
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

        MusicType = sceneData.Music;
        
        return this;
    }

    public void PlayMusic()
    {
        Application.Instance.GetManager<AudioManager>().PlayMusic(MusicType);
    }
    
    public void SpawnPlayerAt(string spawn)
    {
        var door = Doors.FirstOrDefault(d => d.Id == spawn);
        
        if (door != null)
        {
            door.SpawnPlayer();
        }
        else if (Bonfire != null && Bonfire.Spawn.Id == spawn)
        {
            Bonfire.Spawn.SpawnPlayer();
        }
    }

    private void Update()
    {
        foreach (var material in Materials)
        {
            material.SetVector("_LightSourcePosition", LightSource.Source.position); 
            material.SetFloat("_LightSourceRadius", LightSource.Radius); 
            material.SetColor("_LightSourceColor", LightSource.Color); 
        }
    }

    private void OnValidate()
    {
        foreach (var material in Materials)
        {
            material.SetVector("_LightSourcePosition", LightSource.Source?.position ?? Vector3.zero); 
            material.SetFloat("_LightSourceRadius", LightSource.Radius); 
            material.SetColor("_LightSourceColor", LightSource.Color); 
        }
    }
}

[Serializable]
public struct LightSource
{
    [field: SerializeField] public Transform Source { get; private set; }
    [field: SerializeField] public float Radius { get; private set; }
    [field: SerializeField] [field: ColorUsage(false, true)] public Color Color { get; private set; }
}
