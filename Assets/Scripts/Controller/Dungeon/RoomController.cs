using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [field:SerializeField] public List<CorridorAreaController> Doors { get; private set; }
    [field:SerializeField] public List<BonfireController> Bonfires { get; private set; }
    [field:SerializeField] public Camera PreviewCamera { get; private set; }
    
    public RoomController Init(SceneNode sceneData)
    {
        foreach (var door in Doors)
        {
            var doorSpawnData = sceneData.Doors.First(d => d.Id == door.Id);
            door.Spawn = doorSpawnData.ToDomain();
        }

        foreach (var bonfire in Bonfires)
        {
            bonfire.Init(sceneData.Id);
        }
        
        Destroy(PreviewCamera.gameObject);

        return this;
    }
    
    public RoomController Init(DungeonRoomData roomData)
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
    
    public RoomController Init(SceneNodeData roomData)
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
    
    public void SpawnPlayerAt(string spawn)
    {
        var door = Doors.FirstOrDefault(d => d.Id == spawn);
        
        if (door != null)
        {
            Application.Instance.PlayerManager.SpawnPlayerAt(door);
        }
        
        var bonfire = Bonfires.First(d => d.Spawn.Id == spawn);
        
        Application.Instance.PlayerManager.SpawnPlayerAt(bonfire.Spawn);
    }
}
