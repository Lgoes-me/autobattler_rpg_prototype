using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScene : BaseScene
{
    [field: SerializeField] private string SceneName { get; set; }
    
    [field: SerializeField] public MusicType Music { get; private set; }
    [field: SerializeField] private List<DoorController> Doors { get; set; }
    [field: SerializeField] private List<BonfireController> Bonfires { get; set; }

    public void ActivateRoomScene()
    {
        InitBonfires();
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
            var bonfire = Bonfires.First(b => b.Id == spawnSpawnId);
            Application.Instance.PlayerManager.SpawnPlayerAt(bonfire.SpawnPoint);
            bonfire.ActivateCameraArea();
        }
    }

    private void InitBonfires()
    {
        foreach (var bonfire in Bonfires)
        {
            bonfire.Init(SceneName);
        }
    }

}