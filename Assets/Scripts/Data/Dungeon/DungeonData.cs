using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class DungeonData : ScriptableObject
{
    [field: SerializeField] private List<DungeonRoomData> AvailableRooms { get; set; }

    [field: SerializeField] private int MaximumDoors { get; set; }
    [field: SerializeField] private int MinimumDeepness { get; set; }
    [field: SerializeField] private int MaximumDeepness { get; set; }

    public Tree<DungeonRoom> GenerateDungeon()
    {
        var dungeonEntrance = InstantiateRoom(0, 0);
        dungeonEntrance.SetAsEntrance(AvailableRooms.First(x => x.RoomType is RoomType.Entrance));

        var rooms = CreateSubTree(dungeonEntrance, 0);

        while (rooms.Any(r => !r.Data.Collapsed))
        {
            PruneTree(rooms);

            var nextRoom = rooms
                .Where(r => !r.Data.Collapsed)
                .OrderBy(_ => Guid.NewGuid())
                .First();

            nextRoom.Data.SetAsRoom(AvailableRooms.First(x => x.RoomType is RoomType.Normal));
        }

        var bossRoom = rooms
            .Where(r => r.IsLeaf)
            .OrderBy(_ => Guid.NewGuid())
            .First();

        bossRoom.Data.SetAsBossRoom(AvailableRooms.First(x => x.RoomType is RoomType.Boss));

        foreach (var room in rooms)
        {
            var connectedRooms = new List<DungeonRoomData>();
            
            if(!room.IsRoot)
                connectedRooms.Add(room.Parent.Data.SelectedRoom);

            connectedRooms.AddRange(room.ChildrenNodes.Select(t => t.Data.SelectedRoom));
            
            var selectedRoom = room.Data.SelectedRoom;
            
            for (var index = 0; index < selectedRoom.Doors.Count; index++)
            {
                var spawnData = room.Data.SelectedRoom.Doors[index];
                
                if(spawnData.SetUp)
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
        
        return rooms;
    }

    private Tree<DungeonRoom> CreateSubTree(DungeonRoom baseRoom, int level)
    {
        var room = new Tree<DungeonRoom>(baseRoom);

        if (level >= UnityEngine.Random.Range(MinimumDeepness, MaximumDeepness))
            return room;

        level++;

        for (var i = -1; i < MaximumDoors - 2; i++)
        {
            var childRoom = InstantiateRoom(i, level);
            room.Add(CreateSubTree(childRoom, level));
        }

        return room;
    }

    private DungeonRoom InstantiateRoom(int position, int level)
    {
        return new DungeonRoom(position, level);
    }

    private void PruneTree(Tree<DungeonRoom> rooms)
    {
        foreach (var room in rooms)
        {
            if (!room.Data.Collapsed)
                continue;

            if (room.Data.Collapsed && room.Data.SelectedRoom.NumberOfDoors >= room.ChildrenNodes.Count)
                continue;

            var branchesToRemove =
                room
                    .ChildrenNodes
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(room.ChildrenNodes.Count - room.Data.SelectedRoom.NumberOfDoors).ToList();

            foreach (var branch in branchesToRemove)
            {
                room.ChildrenNodes.Remove(branch);
            }
        }
    }
}