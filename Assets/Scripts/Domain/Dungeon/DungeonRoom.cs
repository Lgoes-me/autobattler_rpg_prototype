/*public class DungeonRoom : MonoBehaviour
{
    public bool Collapsed;
    public bool BossRoom;
    public int Doors;
    
    public void SetAsBossRoom()
    {
        Collapsed = true;
        BossRoom = true;
        Doors = 1;
    }
    public void SetAsEntrance()
    {
        Collapsed = true;
        Doors = Random.Range(1, MaxDoors - 1);
    }

    public void SetAsRoom()
    {
        Collapsed = true;
        Doors = Random.Range(1, MaxDoors);
    }
}*/

public class DungeonRoom
{
    public DungeonRoomData SelectedRoom { get; private set; }
    public bool Collapsed => SelectedRoom != null;
    
    public DungeonRoomData SetAsBossRoom()
    {
        return null;
    }

    public DungeonRoomData SetAsEntrance()
    {
        return null;
    }

    public DungeonRoomData SetAsRoom()
    {
        return null;
    }
}