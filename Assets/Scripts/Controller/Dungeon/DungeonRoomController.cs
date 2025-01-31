using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonRoomController : MonoBehaviour
{
    [field:SerializeField] public List<CorridorAreaController> Doors { get; private set; }

    public DungeonRoomController Init(DungeonRoomData roomData)
    {
        foreach (var door in Doors)
        {
            var doorSpawnData = roomData.Doors.First(d => d.Id == door.Id);
            door.DoorDestination = doorSpawnData.DoorDestination;
            door.SceneDestination = doorSpawnData.SceneDestination;
        }

        return this;
    }
}
