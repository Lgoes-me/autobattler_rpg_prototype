using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon
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
    private bool canRepeateRoom;

    public Dungeon(string id, DoorData entrance, DoorData exit, List<DungeonRoomData> availableRooms)
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
        possibilities = RemoveImpossibleRooms(possibilities);

        while (CanBeSimplified(possibilities))
        {
            possibilities = ApplyLowestEntropy(possibilities);
            possibilities = RemoveImpossibleRooms(possibilities);
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
        if (Entrance != null)
        {
            switch (Entrance.Direction)
            {
                case DoorDirection.North:
                {
                    var lastRow = GetRow(possibilities, height);
                    var randomElement = lastRow[UnityEngine.Random.Range(0, lastRow.Length)];
                    randomElement.Entrance = true;
                    RemoveRoomsWithout(randomElement, DoorDirection.South);
                    break;
                }
                case DoorDirection.South:
                {
                    var firstRow = GetRow(possibilities, 0);
                    var randomElement = firstRow[UnityEngine.Random.Range(0, firstRow.Length)];
                    randomElement.Entrance = true;
                    RemoveRoomsWithout(randomElement, DoorDirection.North);
                    break;
                }
                case DoorDirection.West:
                {
                    var firstColumn = GetColumn(possibilities, 0);
                    var randomElement = firstColumn[UnityEngine.Random.Range(0, firstColumn.Length)];
                    randomElement.Entrance = true;
                    RemoveRoomsWithout(randomElement, DoorDirection.East);
                    break;
                }
                case DoorDirection.East:
                {
                    var lastColumn = GetColumn(possibilities, width);
                    var randomElement = lastColumn[UnityEngine.Random.Range(0, lastColumn.Length)];
                    randomElement.Entrance = true;
                    RemoveRoomsWithout(randomElement, DoorDirection.West);
                    break;
                }
            }
        }

        if (Exit != null)
        {
            switch (Exit.Direction)
            {
                case DoorDirection.North:
                {
                    var lastRow = GetRow(possibilities, height);
                    var randomElement = lastRow[UnityEngine.Random.Range(0, lastRow.Length)];
                    randomElement.Exit = true;
                    RemoveRoomsWithout(randomElement, DoorDirection.South);
                    break;
                }
                case DoorDirection.South:
                {
                    var firstRow = GetRow(possibilities, 0);
                    var randomElement = firstRow[UnityEngine.Random.Range(0, firstRow.Length)];
                    randomElement.Exit = true;
                    RemoveRoomsWithout(randomElement, DoorDirection.North);
                    break;
                }
                case DoorDirection.West:
                {
                    var firstColumn = GetColumn(possibilities, 0);
                    var randomElement = firstColumn[UnityEngine.Random.Range(0, firstColumn.Length)];
                    randomElement.Exit = true;
                    RemoveRoomsWithout(randomElement, DoorDirection.East);
                    break;
                }
                case DoorDirection.East:
                {
                    var lastColumn = GetColumn(possibilities, width);
                    var randomElement = lastColumn[UnityEngine.Random.Range(0, lastColumn.Length)];
                    randomElement.Exit = true;
                    RemoveRoomsWithout(randomElement, DoorDirection.West);
                    break;
                }
            } 
        }

        return possibilities;
    }

    private Possibility[,] RemoveImpossibleRooms(Possibility[,] possibilities)
    {
        throw new System.NotImplementedException();
    }

    private Possibility[,] ApplyLowestEntropy(Possibility[,] possibilities)
    {
        throw new System.NotImplementedException();
    }

    private Possibility[] GetColumn(Possibility[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
            .Select(x => matrix[x, columnNumber])
            .Where(p => !p.Entrance && !p.Exit)
            .ToArray();
    }

    private Possibility[] GetRow(Possibility[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
            .Select(x => matrix[rowNumber, x])
            .Where(p => !p.Entrance && !p.Exit)
            .ToArray();
    }
    
    private void RemoveRoomsWithout(Possibility possibility, DoorDirection direction)
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