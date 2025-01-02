public class Dungeon
{
    private Tree<DungeonRoom> Rooms { get; set; }

    public Dungeon(Tree<DungeonRoom> rooms)
    {
        Rooms = rooms;
    }
}