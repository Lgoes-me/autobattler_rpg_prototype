using System.Collections.Generic;

public class DungeonRoomNode : SceneNode, IDungeonRoom
{
    public RoomType RoomType { get; private set; }
    public bool Collapsed { get; private set; }
    public bool Connected { get; set; }

    public DungeonRoomNode()
    {
        RoomType = RoomType.Unknown;
        Collapsed = false;
        Connected = false;
    }

    public DungeonRoomNode(
        string id, 
        List<Transition> doors, 
        RoomController roomPrefab, 
        RoomType roomType) : base(id, doors, roomPrefab, new List<CombatEncounterData>(), null)
    {
        RoomType = roomType;
        Collapsed = false;
        Connected = false;
    }

    public void SetValue(IDungeonRoom dungeonRoom)
    {
        if (dungeonRoom is not DungeonRoomNode room)
            return;
        
        //Id = Guid.NewGuid().ToString();
        //RoomPrefab = room.RoomPrefab;
        RoomType = room.RoomType;
        Collapsed = true;
    }
}