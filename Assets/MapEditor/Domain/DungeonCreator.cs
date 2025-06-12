using System;
using System.Collections.Generic;
using System.Linq;

public class DungeonCreator
{
    public DungeonRoom[,] Rooms { get; set; }
    public Dictionary<string, DungeonRoom> GetRoom { get; private set; }
    public bool Generated { get; private set; }

    private string Id { get; }
    private DoorData Entrance { get; }
    private DoorData Exit { get; }
    private List<DungeonRoomData> AvailableRooms { get; }

    private int height;
    private int width;

    public DungeonCreator(string id, DoorData entrance, DoorData exit, List<DungeonRoomData> availableRooms)
    {
        Rooms = new DungeonRoom[5, 5];
        GetRoom = new Dictionary<string, DungeonRoom>();
        Generated = false;

        Id = id;
        Entrance = entrance;
        Exit = exit;
        AvailableRooms = availableRooms;
    }

    public void Clear()
    {
        Rooms = new DungeonRoom[5, 5];
        GetRoom = new Dictionary<string, DungeonRoom>();
        Generated = false;
    }

    public void GenerateDungeon()
    {
        var possibilities = new Possibility[5, 5];

        for (var i = 0; i < possibilities.GetLength(0); i++)
        for (var j = 0; j < possibilities.GetLength(1); j++)
        {
            possibilities[i, j] = new Possibility(AvailableRooms);
        }

        possibilities = ApplyEntranceAndExit(possibilities);

        while (CanBeSimplified(possibilities))
        {
            possibilities = RemoveImpossibleRooms(possibilities);
            possibilities = ApplyLowestEntropy(possibilities);
        }

        //Apply possibilities and connect rooms

        Generated = true;
    }

    private bool CanBeSimplified(Possibility[,] possibilities)
    {
        for (var i = 0; i < possibilities.GetLength(0); i++)
        for (var j = 0; j < possibilities.GetLength(1); j++)
        {
            if (possibilities[i, j].Rooms.Count == 1)
                continue;

            return true;
        }

        return false;
    }

    private Possibility[,] ApplyEntranceAndExit(Possibility[,] possibilities)
    {
        var entranceOptions = GetRowOrColumnBasedOnDirection(possibilities, Entrance.Direction);
        var randomEntrance = entranceOptions[UnityEngine.Random.Range(0, entranceOptions.Length)];
        randomEntrance.Entrance = true;

        var exitOptions = GetRowOrColumnBasedOnDirection(possibilities, Exit.Direction);
        var randomExit = entranceOptions[UnityEngine.Random.Range(0, exitOptions.Length)];
        randomExit.Exit = true;

        var firstRow = GetRowOrColumnBasedOnDirection(possibilities, DoorDirection.South);
        RemoveRoomsWithout(firstRow, DoorDirection.North);

        var lastRow = GetRowOrColumnBasedOnDirection(possibilities, DoorDirection.North);
        RemoveRoomsWithout(lastRow, DoorDirection.South);

        var firstColumn = GetRowOrColumnBasedOnDirection(possibilities, DoorDirection.West);
        RemoveRoomsWithout(firstColumn, DoorDirection.East);

        var lastColumn = GetRowOrColumnBasedOnDirection(possibilities, DoorDirection.East);
        RemoveRoomsWithout(lastColumn, DoorDirection.West);

        return possibilities;
    }

    private Possibility[,] RemoveImpossibleRooms(Possibility[,] possibilities)
    {
        while (HasImpossibleRooms(possibilities))
        {
            for (var i = 0; i < possibilities.GetLength(0); i++)
            for (var j = 0; j < possibilities.GetLength(1); j++)
            {
                var possibility = possibilities[i, j];
                
                
            }
        }

        return possibilities;
    }

    private bool HasImpossibleRooms(Possibility[,] possibilities)
    {
        return true;
    }
    
    private Possibility[,] ApplyLowestEntropy(Possibility[,] possibilities)
    {
        throw new System.NotImplementedException();
    }
    
    private Possibility GetRoomWithLowestEntropy(Possibility[,] possibilities)
    {
        var item = possibilities[0, 0];

        for (var i = 0; i < possibilities.GetLength(0); i++)
        for (var j = 0; j < possibilities.GetLength(1); j++)
        {
            if (possibilities[i, j].Rooms.Count >= item.Rooms.Count)
                continue;

            item = possibilities[i, j];
        }

        return item;
    }

    private Possibility[] GetRowOrColumnBasedOnDirection(Possibility[,] possibilities, DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.North => GetRowOptionsForEntranceAndExit(possibilities, height),
            DoorDirection.South => GetRowOptionsForEntranceAndExit(possibilities, 0),
            DoorDirection.West => GetColumnOptionsForEntranceAndExit(possibilities, 0),
            DoorDirection.East => GetColumnOptionsForEntranceAndExit(possibilities, width),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private Possibility[] GetColumnOptionsForEntranceAndExit(Possibility[,] possibilities, int columnNumber)
    {
        return Enumerable.Range(0, possibilities.GetLength(0))
            .Select(x => possibilities[x, columnNumber])
            .Where(p => !p.Entrance && !p.Exit)
            .ToArray();
    }

    private Possibility[] GetRowOptionsForEntranceAndExit(Possibility[,] possibilities, int rowNumber)
    {
        return Enumerable.Range(0, possibilities.GetLength(1))
            .Select(x => possibilities[rowNumber, x])
            .Where(p => !p.Entrance && !p.Exit)
            .ToArray();
    }

    private void RemoveRoomsWithout(Possibility[] possibilities, DoorDirection direction)
    {
        foreach (var possibility in possibilities)
        {
            for (int i = possibility.Rooms.Count - 1; i >= 0; i--)
            {
                var room = possibility.Rooms[i];

                if (room.RoomPrefab.Doors.Any(c => c.Direction == direction))
                    continue;

                possibility.Rooms.Remove(room);
            }
        }
    }
}

public class Possibility
{
    public List<DungeonRoomData> Rooms { get; set; }
    public bool Entrance { get; set; }
    public bool Exit { get; set; }

    public Possibility(List<DungeonRoomData> rooms)
    {
        Rooms = rooms;
        Entrance = false;
        Exit = false;
    }
}