using System.Collections.Generic;

public abstract class BaseSceneNode
{
    public string Id { get; protected set; }
    public List<Transition> Doors { get; protected set; }
    public MusicType Music { get; }
    protected string Name { get; set; }

    protected BaseSceneNode()
    {
        Name = string.Empty;
        Id = string.Empty;
        Doors = new List<Transition>();
        Music = MusicType.Dungeon;
    }
    
    protected BaseSceneNode(string name, string id, List<Transition> doors) : this()
    {
        Name = name;
        Id = id;
        Doors = doors;
    }

    public abstract void DoTransition(Spawn spawn, Map map);
}