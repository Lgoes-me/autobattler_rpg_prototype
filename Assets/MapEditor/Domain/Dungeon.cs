using System;
using System.Collections.Generic;
using System.Linq;

public class Dungeon<T> where T : IDungeonRoom, new()
{
    public Tree<T> Rooms { get; set; }
    public Dictionary<string, T> GetRoom { get; private set; }
    public bool Generated { get; private set; }
    
    private DoorData EntranceDoor { get; }
    private List<T> AvailableRooms { get; }
    private int MaximumDoors { get; }
    private int MinimumDeepness { get; }
    private int MaximumDeepness { get; }
    
    public Dungeon(DoorData entranceDoor, List<T> availableRooms, int maximumDoors, int minimumDeepness, int maximumDeepness)
    {
        GetRoom = new Dictionary<string, T>();
        Generated = false;
        Rooms = null;

        EntranceDoor = entranceDoor;
        AvailableRooms = availableRooms;
        MaximumDoors = maximumDoors;
        MinimumDeepness = minimumDeepness;
        MaximumDeepness = maximumDeepness;
    }
    public void Clear()
    {
        GetRoom = new Dictionary<string, T>();
        Generated = false;
        Rooms = null;
    }

    public void GenerateDungeon()
    {
        var dungeonEntrance = new T();
        var entrance = AvailableRooms.First(x => x.RoomType is RoomType.Entrance);
        dungeonEntrance.SetValue(entrance);

        dungeonEntrance.Doors[0].Connect(EntranceDoor.SceneDestination, EntranceDoor.DoorDestination, EntranceDoor.Open);
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
                room.Data.SetValue(bossRoom);
            }
            else
            {
                var anyRoom = AvailableRooms.First(x => x.RoomType is RoomType.Normal);
                room.Data.SetValue(anyRoom);
            }
            
            PruneTree(room);
        }

        foreach (var room in rooms)
        {
            var connectedRooms = new List<T>();

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
                
                spawnData.Connect(connectedRoom.Id, connectedDoorSpawnData.Id, true);
                connectedDoorSpawnData.Connect(selectedRoom.Id, spawnData.Id, true);
            }

            GetRoom.Add(room.Data.Id, room.Data);
        }

        Generated = true;
        Rooms = rooms;
    }

    private Tree<T> CreateSubTree(T baseRoom, int level)
    {
        var room = new Tree<T>(baseRoom);

        if (level >= UnityEngine.Random.Range(MinimumDeepness, MaximumDeepness))
            return room;

        level++;

        for (var i = -1; i < MaximumDoors - 2; i++)
        {
            var childRoom = new T();
            room.Add(CreateSubTree(childRoom, level));
        }

        return room;
    }

    private void PruneTree(Tree<T> room)
    {
        if (!room.Data.Collapsed)
            return;

        if (room.Data.Collapsed && room.Data.NumberOfDoors - 1 >= room.ChildrenNodes.Count)
            return;

        var branchesToRemove =
            room
                .ChildrenNodes
                .OrderBy(_ => Guid.NewGuid())
                .Take(room.ChildrenNodes.Count - room.Data.NumberOfDoors).ToList();

        foreach (var branch in branchesToRemove)
        {
            room.ChildrenNodes.Remove(branch);
        }
    }
}

public interface IDungeonRoom
{
    string Id { get; }
    RoomType RoomType { get; }
    List<DoorData> Doors { get; }
    bool Collapsed { get; }
    bool Connected { get; set; }
    
    int NumberOfDoors => Doors.Count;

    void SetValue(IDungeonRoom dungeonRoom);
}