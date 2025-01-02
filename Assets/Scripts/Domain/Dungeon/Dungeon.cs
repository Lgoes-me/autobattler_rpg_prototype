using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public DungeonRoomData DungeonRoomData;
    private Tree<DungeonRoom> Rooms { get; set; }
}