using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] public MusicType Music { get; private set; }
    private DungeonRoomController DungeonRoomController { get; set; }

    public void ActivateRoomScene(DungeonRoomData roomData)
    {
        DungeonRoomController = Instantiate(roomData.RoomPrefab).Init(roomData);
    }
    
    public void ActivateRoomScene(SceneNodeData roomData)
    {
        DungeonRoomController = Instantiate(roomData.RoomPrefab).Init(roomData);
    }

    public void SpawnPlayerAt(string spawnSpawnId)
    {
        DungeonRoomController.SpawnPlayerAt(spawnSpawnId);
    }
}