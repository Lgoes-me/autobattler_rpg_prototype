using System.Collections.Generic;

public abstract class BaseSceneNode
{
    public string Id { get; }
    public string Name { get; }
    public List<SpawnData> Doors { get; }

    protected BaseSceneNode(string name, string id, List<SpawnData> doors)
    {
        Name = name;
        Id = id;
        Doors = doors;
    }
}

public class SceneNode : BaseSceneNode
{
    public RoomController RoomPrefab { get; }

    public SceneNode(string name, string id, List<SpawnData> doors, RoomController roomPrefab) : base(name, id, doors)
    {
        RoomPrefab = roomPrefab;
    }
}

public class DungeonSceneNode : SceneNode, IDungeonRoom
{
    public RoomType RoomType { get; }

    public DungeonSceneNode(string name, string id, List<SpawnData> doors, RoomController roomPrefab, RoomType roomType)
        : base(name, id, doors, roomPrefab)
    {
        RoomType = roomType;
    }
}

public class DungeonNode : BaseSceneNode
{
    public Dungeon<DungeonSceneNode> Dungeon { get; }

    public DungeonNode(string name, string id, List<SpawnData> doors, List<DungeonSceneNode> availableRooms, 
        int maximumDoors, int minimumDeepness, int maximumDeepness) : base(name, id, doors)
    {
        Dungeon = new Dungeon<DungeonSceneNode>(availableRooms, maximumDoors, minimumDeepness, maximumDeepness);
    }

    public SceneNode GetRoom(SpawnDomain spawn)
    {
        throw new System.NotImplementedException();
    }

    public void GenerateDungeon()
    {
        Dungeon.GenerateDungeon();
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