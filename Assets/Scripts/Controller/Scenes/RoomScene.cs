using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] private string SceneName { get; set; }
    
    [field: SerializeField] public MusicType Music { get; private set; }
    [field: SerializeField] private List<DoorController> Doors { get; set; }
    [field: SerializeField] private List<BonfireController> Bonfires { get; set; }
    [field: SerializeField] private List<EnemyAreaController> EnemyAreas { get; set; }

    public void ActivateRoomScene(SceneManager sceneManager, PlayerController playerController)
    {
        InitDoors(sceneManager);
        InitBonfires(sceneManager);
        InitEnemyAreas(sceneManager, playerController);
    }

    public void SpawnPlayerAt(string spawnSpawnId)
    {
        var door = Doors.FirstOrDefault(d => d.DoorName == spawnSpawnId);
        
        if (door != null)
        {
            door.ActivateCameraArea();
            Application.Instance.PlayerManager.SpawnPlayerAt(door.SpawnPoint);
        }
        else
        {
            var bonfire = Bonfires.First(b => b.Id == spawnSpawnId);;
            bonfire.Interact();
            
            bonfire.ActivateCameraArea();
            Application.Instance.PlayerManager.SpawnPlayerAt(bonfire.SpawnPoint);
        }
        
    }

    private void InitEnemyAreas(SceneManager sceneManager, PlayerController playerController)
    {
        foreach (var enemyArea in EnemyAreas)
        {
            enemyArea.Init(sceneManager, playerController);
        }
    }

    private void InitDoors(SceneManager sceneManager)
    {
        foreach (var door in Doors)
        {
            door.Init(sceneManager);
        }
    }
    
    private void InitBonfires(SceneManager sceneManager)
    {
        foreach (var bonfire in Bonfires)
        {
            bonfire.Init(sceneManager, SceneName);
        }
    }

}