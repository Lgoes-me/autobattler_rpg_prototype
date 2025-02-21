using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] public MusicType Music { get; private set; }
    private RoomController RoomController { get; set; }

    public void ActivateRoomScene(SceneNode sceneData)
    {
        RoomController = Instantiate(sceneData.RoomPrefab).Init(sceneData);
    }
    
    public void SpawnPlayerAt(string spawn)
    {
        RoomController.SpawnPlayerAt(spawn);
    }
}