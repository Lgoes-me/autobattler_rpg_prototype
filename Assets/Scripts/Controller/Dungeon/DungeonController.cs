using UnityEngine;

public class DungeonController : MonoBehaviour
{
    [field: SerializeField] private DungeonData DungeonData { get; set; }
    private Dungeon Dungeon { get; set; }

    private void Start()
    {
        var rooms = DungeonData.GenerateDungeon();
        Dungeon = new Dungeon(rooms);
    }
}