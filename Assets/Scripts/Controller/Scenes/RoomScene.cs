using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] public MusicType Music { get; private set; }
    private RoomController RoomController { get; set; }

    public void ActivateRoomScene(DungeonRoomData roomData)
    {
        RoomController = Instantiate(roomData.RoomPrefab).Init(roomData);
    }
    
    public void ActivateRoomScene(SceneNodeData roomData)
    {
        RoomController = Instantiate(roomData.RoomPrefab).Init(roomData);
    }

    public void SpawnPlayerAtDoor(string spawnSpawnId)
    {
        RoomController.SpawnPlayerAtDoor(spawnSpawnId);
    }

    public void SpawnPlayerAtBonfire(string bonfireSpawnId)
    {
        RoomController.SpawnPlayerAtBonfire(bonfireSpawnId);
    }
}