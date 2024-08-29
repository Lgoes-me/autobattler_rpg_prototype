using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] private List<DoorController> Doors { get; set; }
    [field: SerializeField] private List<EnemyAreaController> EnemyAreas { get; set; }

    public void ActivateRoomScene(string doorName)
    {
        InitEnemyAreas();
        SpawnPlayer(doorName);
    }

    private void SpawnPlayer(string doorName)
    {
        var door = Doors.First(d => d.DoorName == doorName);
        door.ActivateCameraArea();
        Application.Instance.PlayerManager.SpawnPlayerAt(door.SpawnPoint);
    }

    private void InitEnemyAreas()
    {
        var player = Application.Instance.PlayerManager.PlayerController;

        foreach (var enemyArea in EnemyAreas)
        {
            enemyArea.Init(player);
        }
    }
}