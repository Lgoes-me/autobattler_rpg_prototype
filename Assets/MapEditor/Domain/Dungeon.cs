using System;
using System.Collections.Generic;
using System.Linq;

public class Dungeon 
{
    public DungeonRoom[,] Rooms { get; set; }
    public Dictionary<string, DungeonRoom> GetRoom { get; private set; }
    public bool Generated { get; private set; }
    
    private string Id { get; }
    private Transition Entrance { get; }
    private Transition Exit { get; }
    private List<DungeonRoomData> AvailableRooms { get; }
    
    public Dungeon(string id, Transition entrance, Transition exit, List<DungeonRoomData> availableRooms)
    {
        GetRoom = new Dictionary<string, DungeonRoom>();
        Generated = false;
        Rooms = null;

        Id = id;
        Entrance = entrance;
        Exit = exit;
        AvailableRooms = availableRooms;
    }
    public void Clear()
    {
        GetRoom = new Dictionary<string, DungeonRoom>();
        Generated = false;
        Rooms = null;
    }

    public void GenerateDungeon()
    {
        /*var dungeonEntrance = new DungeonRoom();
        var entrance = AvailableRooms.First(x => x.RoomType is RoomType.Entrance);
        dungeonEntrance.SetValue(entrance.ToDomain(Id));

        //dungeonEntrance.Doors[0].Connect(EntranceDoor.SceneDestination, EntranceDoor.DoorDestination);
        dungeonEntrance.Connected = true;
        
        var rooms = CreateSubTree(dungeonEntrance, 0);
        
        PruneTree(rooms);
       
        while (rooms.Any(r => !r.Data.Collapsed))
        {
            var room = rooms
                .Where(r => !r.Data.Collapsed)
                .OrderBy(_ => Guid.NewGuid())
                .First();

            if (room.IsLeaf)
            {
                var bossRoom = AvailableRooms.First(x => x.RoomType is RoomType.Boss);
                room.Data.SetValue(bossRoom.ToDomain(Id));
            }
            else
            {
                var anyRoom = AvailableRooms.First(x => x.RoomType is RoomType.Normal);
                room.Data.SetValue(anyRoom.ToDomain(Id));
            }
            
            PruneTree(room);
        }

        foreach (var room in rooms)
        {
            var connectedRooms = new List<DungeonRoom>();

            if (!room.IsRoot)
                connectedRooms.Add(room.Parent.Data);
           
            var children = room.ChildrenNodes.Select(t => t.Data);
            connectedRooms.AddRange(children);

            var selectedRoom = room.Data;

            for (var index = 0; index < selectedRoom.Doors.Count; index++)
            {
                var spawnData = room.Data.Doors[index];

                if (spawnData.SetUp)
                    continue;

                var connectedRoom = connectedRooms.First(d => !d.Connected);
                var connectedDoorSpawnData = connectedRoom.Doors.First(s => !s.SetUp);

                connectedRoom.Connected = true;
                
                spawnData.Connect(connectedRoom.Id, connectedDoorSpawnData.Id);
                connectedDoorSpawnData.Connect(selectedRoom.Id, spawnData.Id);
            }

            GetRoom.Add(room.Data.Id, room.Data);
        }

        Generated = true;
        Rooms = rooms;*/
    }
}