using System;
using System.Collections.Generic;
using System.Linq;

public class Map
{
    internal Dictionary<string, BaseNode> AllNodesById { get; set; }

    public Map(List<BaseNode> nodes)
    {
        AllNodesById = nodes.ToDictionary(n => n.Id, n => n);
    }

    public void SpawnAt(string name, Action<BaseSceneNode, Spawn> callback)
    {
        var node = (SpawnNode) AllNodesById[name];
        var spawn = node.Spawn.Destination;

        node.DoTransition(this, spawn, callback);
    }

    public void ChangeContext(Spawn spawn, Action<BaseSceneNode, Spawn> callback)
    {        
        var node = AllNodesById[spawn.NodeId];
        node.DoTransition(this, spawn, callback);
    }

    public bool IsOpen(Transition transition)
    {
        var spawn = transition.Destination;
        var node = AllNodesById[spawn.NodeId];
        
        return node.IsOpen(this, spawn);
    }

    public DialogueData GetDialogue(Transition transition)
    {
        var spawn = transition.Destination;
        var node = AllNodesById[spawn.NodeId];
        
        return node.GetDialogue(this, spawn);
    }
}