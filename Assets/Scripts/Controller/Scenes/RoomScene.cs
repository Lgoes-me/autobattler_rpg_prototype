using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] private List<DoorController> Doors { get; set; }
    [field: SerializeField] private List<EnemyAreaController> EnemyAreas { get; set; }

    public void ActivateRoomScene(SceneManager sceneManager, PlayerController playerController, string doorName)
    {
        InitDoors(sceneManager);
        InitEnemyAreas(sceneManager, playerController);
        SpawnPlayer(doorName);
    }

    private void SpawnPlayer(string doorName)
    {
        var door = Doors.First(d => d.DoorName == doorName);
        door.ActivateCameraArea();
        Application.Instance.PlayerManager.SpawnPlayerAt(door.SpawnPoint);
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
}