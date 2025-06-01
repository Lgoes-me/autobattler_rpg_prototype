using System;
using System.Collections.Generic;
using System.Linq;

public class Map
{
    internal Dictionary<string, BaseSceneNode> AllNodesById { get; set; }

    public Map(List<BaseSceneNode> nodes)
    {
        AllNodesById = nodes.ToDictionary(n => n.Id, n => n);
    }

    public void SpawnAt(string name, Action<SceneNode, Spawn> callback)
    {
        var node = AllNodesById[name];
        var spawn = node.Doors[0].Destination;

        node.DoTransition(this, spawn, callback);
    }

    public void ChangeContext(Spawn spawn, Action<SceneNode, Spawn> callback)
    {        
        var node = AllNodesById[spawn.NodeId];
        node.DoTransition(this, spawn, callback);
    }
    
    public void ChangeContext(Transition transition, Action<SceneNode, Spawn> callback)
    {
        var spawn = transition.Destination;
        var node = AllNodesById[spawn.NodeId];
        
        node.DoTransition(this, spawn, callback);
    }

    public bool IsOpen(Transition transition)
    {
        var spawn = transition.Destination;
        var node = AllNodesById[spawn.NodeId];
        
        return node.IsOpen();
    }
}