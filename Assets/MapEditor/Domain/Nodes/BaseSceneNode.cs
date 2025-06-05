using System;
using System.Collections.Generic;

public abstract class BaseSceneNode
{
    public string Id { get; }

    protected BaseSceneNode()
    {
        Id = string.Empty;
    }
    
    protected BaseSceneNode(string id) : this()
    {
        Id = id;
    }

    public abstract void DoTransition(Map map, Spawn spawn, Action<SceneData, Spawn> callback);
    public abstract bool IsOpen(Map map, Spawn spawn);
}