public class DungeonRoom
{
    public DungeonRoomData SelectedRoom { get; private set; }
    public bool Collapsed => SelectedRoom != null;

    public int Position;
    public int Level;
    
    public DungeonRoom(int position, int level)
    {
        Position = position;
        Level = level;
    }

    public DungeonRoomData SetAsBossRoom(DungeonRoomData dungeonRoomData)
    {
        SelectedRoom = dungeonRoomData;
        return null;
    }

    public DungeonRoomData SetAsEntrance(DungeonRoomData dungeonRoomData)
    {
        SelectedRoom = dungeonRoomData;
        return null;
    }

    public DungeonRoomData SetAsRoom(DungeonRoomData dungeonRoomData)
    {
        SelectedRoom = dungeonRoomData;
        return null;
    }
}