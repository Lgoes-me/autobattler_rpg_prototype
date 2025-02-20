using System.Collections.Generic;

public abstract class BaseSceneNode
{
    public string Id { get; protected set; }
    public string Name { get; protected set; }
    public List<SpawnData> Doors { get; protected set; }

    protected BaseSceneNode()
    {
        Name = string.Empty;
        Id = string.Empty;
        Doors = new List<SpawnData>();
    }
    
    protected BaseSceneNode(string name, string id, List<SpawnData> doors) : this()
    {
        Name = name;
        Id = id;
        Doors = doors;
    }
}

public class SceneNode : BaseSceneNode
{
    public RoomController RoomPrefab { get; protected set; }
    
    protected SceneNode() : base()
    {
        RoomPrefab = null;
    }
    
    public SceneNode(string name, string id, List<SpawnData> doors, RoomController roomPrefab) : base(name, id, doors)
    {
        RoomPrefab = roomPrefab;
    }
}

public class DungeonSceneNode : SceneNode, IDungeonRoom
{
    public RoomType RoomType { get; private set; }
    public bool Collapsed { get; private set; }

    public DungeonSceneNode() : base()
    {
        RoomType = RoomType.Unknown;
        Collapsed = false;
    }

    public DungeonSceneNode(
        string name, 
        string id, 
        List<SpawnData> doors, 
        RoomController roomPrefab, 
        RoomType roomType) : base(name, id, doors, roomPrefab)
    {
        RoomType = roomType;
    }

    public void SetValue(IDungeonRoom dungeonRoom)
    {
        if (dungeonRoom is not DungeonSceneNode room)
            return;
        
        Name = room.Name;
        Id = room.Id;
        Doors = room.Doors;
        RoomPrefab = room.RoomPrefab;
        RoomType = room.RoomType;
        Collapsed = true;
    }
}

public class DungeonNode : BaseSceneNode
{
    private Dungeon<DungeonSceneNode> Dungeon { get; }

    public DungeonNode(string name, string id, List<SpawnData> doors, List<DungeonSceneNode> availableRooms, 
        int maximumDoors, int minimumDeepness, int maximumDeepness) : base(name, id, doors)
    {
        Dungeon = new Dungeon<DungeonSceneNode>(availableRooms, maximumDoors, minimumDeepness, maximumDeepness);
    }

    public SceneNode GetRoom(SpawnDomain spawn)
    {
        if (!Dungeon.Generated)
        {
            Dungeon.GenerateDungeon();
        }

        return Dungeon.GetRoom[spawn.SceneId];
    }

    public void Clear()
    {
        Dungeon.Clear();
    }
}

public class SpawnNode : BaseSceneNode
{
    public SpawnNode(string name, string id, List<SpawnData> doors) : base(name, id, doors)
    {
    }
}

public class GameEventNode : BaseSceneNode
{
    public GameAction GameAction { get; }

    public GameEventNode(string name, string id, List<SpawnData> doors, GameAction gameAction) : base(name, id, doors)
    {
        GameAction = gameAction;
    }

    public void DoAction()
    {
        GameAction.Invoke();
    }
}