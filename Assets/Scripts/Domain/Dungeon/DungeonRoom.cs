public class DungeonRoom<T>
{
    public T SelectedRoom { get; private set; }
    public int NumberOfDoors { get; private set; }
    public bool Collapsed => SelectedRoom != null;

    public int Position;
    public int Level;
    
    public DungeonRoom(int position, int level)
    {
        Position = position;
        Level = level;
    }

    public DungeonRoomData SetRoom(T dungeonRoomData, int doors)
    {
        SelectedRoom = dungeonRoomData;
        NumberOfDoors = doors;
        return null;
    }
}