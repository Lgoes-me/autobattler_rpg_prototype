using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [field: SerializeField] public DungeonData DungeonData { get; set; }
    private Tree<DungeonRoom> Rooms { get; set; }

    private void Start()
    {
        Rooms = DungeonData.GenerateDungeon();
    }
}