using System;
using System.Collections.Generic;

public abstract class BaseSceneNode
{
    public string Id { get; }
    public List<Transition> Doors { get; }
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

    public abstract void DoTransition(Map map, Spawn spawn, Action<SceneData, Spawn> callback);
    public abstract bool IsOpen(Map map, Spawn spawn);
}