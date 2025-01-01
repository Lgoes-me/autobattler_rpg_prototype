using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public bool Collapsed;
    public bool BossRoom;
    public int Doors;
    public int MaxDoors;
    
    public void SetAsBossRoom()
    {
        Collapsed = true;
        BossRoom = true;
        Doors = 1;
    }

    public void Init(int maxDoors)
    {
        MaxDoors = maxDoors;
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
}