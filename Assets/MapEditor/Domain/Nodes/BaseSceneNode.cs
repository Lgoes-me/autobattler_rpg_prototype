using System.Collections.Generic;
using System.Threading.Tasks;

public abstract class BaseSceneNode
{
    public string Id { get; protected set; }
    public string Name { get; protected set; }
    public List<SpawnDomain> Doors { get; protected set; }
    public MusicType Music { get; private set; }

    protected BaseSceneNode()
    {
        Name = string.Empty;
        Id = string.Empty;
        Doors = new List<SpawnDomain>();
        Music = MusicType.Dungeon;
    }
    
    protected BaseSceneNode(string name, string id, List<SpawnDomain> doors) : this()
    {
        Name = name;
        Id = id;
        Doors = doors;
    }

    public abstract void DoTransition(SpawnDomain spawn, Map map);
}