using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] public MusicType Music { get; private set; }
    private List<SpawnController> Spawns { get; set; }
    private DungeonRoomController DungeonRoomController { get; set; }

    public void ActivateRoomScene(DungeonRoomData roomData)
    {
        DungeonRoomController = Instantiate(roomData.RoomPrefab).Init(roomData);
        Spawns = FindObjectsByType<SpawnController>(FindObjectsSortMode.None).ToList();
    }

    public void SpawnPlayerAt(string spawnSpawnId)
    {
        var door = Spawns.First(d => d.Id == spawnSpawnId);
        Application.Instance.PlayerManager.SpawnPlayerAt(door);
    }
}