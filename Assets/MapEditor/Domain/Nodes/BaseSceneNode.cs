using System;
using System.Collections.Generic;

public abstract class BaseSceneNode
{
    public string Id { get; protected set; }
    public List<Transition> Doors { get; protected set; }
    public MusicType Music { get; }

    protected BaseSceneNode()
    {
        Id = string.Empty;
        Doors = new List<Transition>();
        Music = MusicType.Dungeon;
    }
    
    protected BaseSceneNode(string id, List<Transition> doors) : this()
    {
        Id = id;
        Doors = doors;
    }

    public abstract void DoTransition(Map map, Spawn spawn, Action<SceneNode, Spawn> callback);
    public abstract bool IsOpen();
}