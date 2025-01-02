using UnityEngine;

[CreateAssetMenu]
public class DungeonRoomData : ScriptableObject
{
    [field: SerializeField] public DungeonRoomController RoomPrefab { get; set; }
    [field: SerializeField] public int NumberOfDoors { get; set; }
}