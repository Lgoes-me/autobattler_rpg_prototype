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

    public void SpawnAt(string name, Action<SceneData, Spawn> callback)
    {
        var node = (SpawnNode) AllNodesById[name];
        var spawn = node.Spawn.Destination;

        node.DoTransition(this, spawn, callback);
    }

    public void ChangeContext(Spawn spawn, Action<SceneData, Spawn> callback)
    {        
        var node = AllNodesById[spawn.NodeId];
        node.DoTransition(this, spawn, callback);
    }
    
    public void ChangeContext(Transition transition, Action<SceneData, Spawn> callback)
    {
        var spawn = transition.Destination;
        var node = AllNodesById[spawn.NodeId];
        
        node.DoTransition(this, spawn, callback);
    }

    public bool IsOpen(Transition transition)
    {
        var spawn = transition.Destination;
        var node = AllNodesById[spawn.NodeId];
        
        return node.IsOpen(this, spawn);
    }
}