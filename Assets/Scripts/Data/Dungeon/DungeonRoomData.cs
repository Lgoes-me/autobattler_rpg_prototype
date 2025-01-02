using UnityEngine;

[CreateAssetMenu]
public class DungeonRoomData : ScriptableObject
{
    //[field: SerializeField] public DungeonRoom DungeonPrefab { get; set; }
    [field: SerializeField] public int NumberOfDoors { get; set; }
}