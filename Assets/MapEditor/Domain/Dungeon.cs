using System.Collections.Generic;

public class Dungeon
{
    public DungeonRoom[,] Rooms { get; set; }
    public Dictionary<string, DungeonRoom> GetRoom { get; private set; }
    public bool Generated { get; private set; }

    private string Id { get; }
    private DoorData Entrance { get; }
    private DoorData Exit { get; }
    private List<DungeonRoomData> AvailableRooms { get; }

    public Dungeon(string id, DoorData entrance, DoorData exit, List<DungeonRoomData> availableRooms)
    {
        Rooms = new DungeonRoom[3, 3];
        GetRoom = new Dictionary<string, DungeonRoom>();
        Generated = false;

        Id = id;
        Entrance = entrance;
        Exit = exit;
        AvailableRooms = availableRooms;
    }

    public void Clear()
    {
        Rooms = new DungeonRoom[3, 3];
        GetRoom = new Dictionary<string, DungeonRoom>();
        Generated = false;
    }

    public void GenerateDungeon()
    {
        var possibilities = new List<DungeonRoomData>[3, 3];

        for (var i = 0; i < possibilities.GetLength(0); i++)
        for (var j = 0; j < possibilities.GetLength(1); j++)
        {
            possibilities[i, j] = AvailableRooms;
        }
        
        possibilities = ApplyEntrance(possibilities);
        possibilities = ApplyExit(possibilities);

        possibilities = RemoveImpossibleRooms(possibilities);

        while (CanBeSimplified(possibilities))
        {
            possibilities = ApplyLowestEntropy(possibilities);
            possibilities = RemoveImpossibleRooms(possibilities);
        }
        
        //Apply possibilities and connect rooms

        Generated = true;
    }

    private bool CanBeSimplified(List<DungeonRoomData>[,] possibilities)
    {
        for (var i = 0; i < possibilities.GetLength(0); i++)
        for (var j = 0; j < possibilities.GetLength(1); j++)
        {
            if (possibilities[i, j].Count == 0) 
                continue;
            
            return true;
        }

        return false;
    }

    private List<DungeonRoomData>[,] ApplyEntrance(List<DungeonRoomData>[,] possibilities)
    {
        throw new System.NotImplementedException();
    }
    
    private List<DungeonRoomData>[,] ApplyExit(List<DungeonRoomData>[,] possibilities)
    {
        throw new System.NotImplementedException();
    }

    private List<DungeonRoomData>[,] RemoveImpossibleRooms(List<DungeonRoomData>[,] possibilities)
    {
        throw new System.NotImplementedException();
    }

    private List<DungeonRoomData>[,] ApplyLowestEntropy(List<DungeonRoomData>[,] possibilities)
    {
        throw new System.NotImplementedException();
    }
}