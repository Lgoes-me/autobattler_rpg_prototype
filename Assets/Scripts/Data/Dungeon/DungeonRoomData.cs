using UnityEngine;

[CreateAssetMenu]
public class DungeonRoomData : ScriptableObject
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public DungeonRoomController RoomPrefab { get; set; }
    [field: SerializeField] public RoomType RoomType { get; set; }
    
    public int NumberOfDoors => RoomPrefab.Doors.Count;
}

public enum RoomType
{
    Entrance,
    Normal,
    Boss
}