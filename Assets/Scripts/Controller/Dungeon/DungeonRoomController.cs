using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonRoomController : MonoBehaviour
{
    [field:SerializeField] public List<CorridorAreaController> Doors { get; private set; }
    [field:SerializeField] public List<BonfireController> Bonfires { get; private set; }
    [field:SerializeField] public Camera PreviewCamera { get; private set; }

    public DungeonRoomController Init(DungeonRoomData roomData)
    {
        foreach (var door in Doors)
        {
            var doorSpawnData = roomData.Doors.First(d => d.Id == door.Id);
            door.Spawn = doorSpawnData.ToDomain();
        }

        foreach (var bonfire in Bonfires)
        {
            bonfire.Init(roomData.Id);
        }
        
        Destroy(PreviewCamera.gameObject);

        return this;
    }
    
    
    public DungeonRoomController Init(SceneNodeData roomData)
    {
        foreach (var door in Doors)
        {
            var doorSpawnData = roomData.Doors.First(d => d.Id == door.Id);
            door.Spawn = doorSpawnData.ToDomain();
        }

        foreach (var bonfire in Bonfires)
        {
            bonfire.Init(roomData.Id);
        }
        
        Destroy(PreviewCamera.gameObject);
    
        return this;
    }
    
    public void SpawnPlayerAtDoor(string spawnSpawnId)
    {
        var door = Doors.First(d => d.Id == spawnSpawnId);
        Application.Instance.PlayerManager.SpawnPlayerAt(door);
    }
    
    public void SpawnPlayerAtBonfire(string bonfireSpawnId)
    {
        var bonfire = Bonfires.First(d => d.Spawn.Id == bonfireSpawnId);
        Application.Instance.PlayerManager.SpawnPlayerAt(bonfire.Spawn);
    }
}
