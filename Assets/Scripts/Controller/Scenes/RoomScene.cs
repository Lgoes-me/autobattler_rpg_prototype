using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] private List<DoorController> Doors { get; set; }
    [field: SerializeField] private List<EnemyAreaController> EnemyAreas { get; set; }

    public void ActivateRoomScene(PlayerController playerController, string doorName)
    {
        InitEnemyAreas(playerController);
        SpawnPlayer(doorName);
    }

    private void SpawnPlayer(string doorName)
    {
        var door = Doors.First(d => d.DoorName == doorName);
        door.ActivateCameraArea();
        Application.Instance.PlayerManager.SpawnPlayerAt(door.SpawnPoint);
    }

    private void InitEnemyAreas(PlayerController playerController)
    {
        foreach (var enemyArea in EnemyAreas)
        {
            enemyArea.Init(playerController);
        }
    }
}