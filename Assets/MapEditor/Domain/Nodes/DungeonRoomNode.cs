using System;
using System.Collections.Generic;

public class DungeonRoomNode : SceneNode, IDungeonRoom
{
    public RoomType RoomType { get; private set; }
    public bool Collapsed { get; private set; }
    public bool Connected { get; set; }

    public DungeonRoomNode() : base()
    {
        RoomType = RoomType.Unknown;
        Collapsed = false;
        Connected = false;
    }

    public DungeonRoomNode(
        string name, 
        string id, 
        List<DoorData> doors, 
        RoomController roomPrefab, 
        RoomType roomType) : base(name, id, doors, roomPrefab, new List<CombatEncounterData>())
    {
        RoomType = roomType;
        Collapsed = false;
        Connected = false;
    }

    public void SetValue(IDungeonRoom dungeonRoom)
    {
        if (dungeonRoom is not DungeonRoomNode room)
            return;
        
        Name = room.Name;
        Id = Guid.NewGuid().ToString();
        Doors = room.Doors;
        RoomPrefab = room.RoomPrefab;
        RoomType = room.RoomType;
        Collapsed = true;
    }
}