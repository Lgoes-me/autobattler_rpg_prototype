public class Dungeon
{
    public Tree<DungeonRoom> Rooms { get; set; }
    
    public void GenerateDungeon()
    {
        var entranceRoom = new DungeonRoom();
        
        Rooms = new Tree<DungeonRoom>(entranceRoom)
        {
            new DungeonRoom(),
            new DungeonRoom(),
            new Tree<DungeonRoom>(new DungeonRoom())
            {
                
            },
        };
    }
}