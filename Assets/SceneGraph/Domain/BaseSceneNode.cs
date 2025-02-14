using System.Collections.Generic;
using System.Threading.Tasks;

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

public class DungeonNode : BaseSceneNode
{
    public Dungeon Dungeon { get; }
    
    public DungeonNode(string name, string id, List<SpawnData> doors) : base(name, id, doors)
    {
        
    }

    public SceneNode GetRoom(SpawnDomain spawn)
    {
        throw new System.NotImplementedException();
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

