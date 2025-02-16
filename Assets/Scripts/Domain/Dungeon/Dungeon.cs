using System;
using System.Collections.Generic;
using System.Linq;

public class Dungeon<T> where T : IDungeonRoom
{
    private Tree<DungeonRoom<T>> Rooms { get; set; }
    
    private List<T> AvailableRooms { get; }
    private int MaximumDoors { get; set; }
    private int MinimumDeepness { get; set; }
    private int MaximumDeepness { get; set; }
    
    public Dungeon(List<T> availableRooms, int maximumDoors, int minimumDeepness, int maximumDeepness)
    {
        AvailableRooms = availableRooms;
        MaximumDoors = maximumDoors;
        MinimumDeepness = minimumDeepness;
        MaximumDeepness = maximumDeepness;
    }

    public void GenerateDungeon()
    {
        var dungeonEntrance = new DungeonRoom<T>(0, 0);
        var entrance = AvailableRooms.First(x => x.RoomType is RoomType.Entrance);
        dungeonEntrance.SetRoom(entrance, entrance.Doors.Count);

        var rooms = CreateSubTree(dungeonEntrance, 0);

        while (rooms.Any(r => !r.Data.Collapsed))
        {
            PruneTree(rooms);

            var nextRoom = rooms
                .Where(r => !r.Data.Collapsed)
                .OrderBy(_ => Guid.NewGuid())
                .First();

            var room = AvailableRooms.First(x => x.RoomType is RoomType.Normal);
            nextRoom.Data.SetRoom(room, room.Doors.Count);
        }

        var finalRoom = rooms
            .Where(r => r.IsLeaf)
            .OrderBy(_ => Guid.NewGuid())
            .First();

        var bossRoom = AvailableRooms.First(x => x.RoomType is RoomType.Boss);
        finalRoom.Data.SetRoom(bossRoom, bossRoom.Doors.Count);

        foreach (var room in rooms)
        {
            var connectedRooms = new List<T>();

            if (!room.IsRoot)
                connectedRooms.Add(room.Parent.Data.SelectedRoom);

            connectedRooms.AddRange(room.ChildrenNodes.Select(t => t.Data.SelectedRoom));

            var selectedRoom = room.Data.SelectedRoom;

            for (var index = 0; index < selectedRoom.Doors.Count; index++)
            {
                var spawnData = room.Data.SelectedRoom.Doors[index];

                if (spawnData.SetUp)
                    continue;

                var connectedRoom = connectedRooms[index];
                var connectedDoorSpawnData = connectedRoom.Doors.First(s => !s.SetUp);

                spawnData.SceneDestination = connectedRoom.Id;
                spawnData.DoorDestination = connectedDoorSpawnData.Id;
                spawnData.SetUp = true;

                connectedDoorSpawnData.SceneDestination = selectedRoom.Id;
                connectedDoorSpawnData.DoorDestination = spawnData.Id;
                connectedDoorSpawnData.SetUp = true;
            }
        }

        Rooms = rooms;
    }

    private Tree<DungeonRoom<T>> CreateSubTree(DungeonRoom<T> baseRoom, int level)
    {
        var room = new Tree<DungeonRoom<T>>(baseRoom);

        if (level >= UnityEngine.Random.Range(MinimumDeepness, MaximumDeepness))
            return room;

        level++;

        for (var i = -1; i < MaximumDoors - 2; i++)
        {
            var childRoom = new DungeonRoom<T>(i, level);
            room.Add(CreateSubTree(childRoom, level));
        }

        return room;
    }

    private void PruneTree(Tree<DungeonRoom<T>> rooms)
    {
        foreach (var room in rooms)
        {
            if (!room.Data.Collapsed)
                continue;

            if (room.Data.Collapsed && room.Data.NumberOfDoors >= room.ChildrenNodes.Count)
                continue;

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
}

public interface IDungeonRoom
{
    string Id { get; }
    RoomType RoomType { get; }
    List<SpawnData> Doors { get; }
}