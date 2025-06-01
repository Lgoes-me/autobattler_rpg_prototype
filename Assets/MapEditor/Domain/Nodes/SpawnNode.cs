using System;
using System.Collections.Generic;
using System.Linq;

public class SpawnNode : BaseSceneNode
{
    public SpawnNode(string id, List<Transition> doors) : base(id, doors)
    {
    }

    public override void DoTransition(Map map, Spawn spawn, Action<SceneNode, Spawn> callback)
    {
        var destination = Doors.First().Destination;
        var nextContext = map.AllNodesById[destination.NodeId];
        nextContext.DoTransition(map, destination, callback);
    }

    public override bool IsOpen(Map map, Spawn spawn)
    {
        return false;
    }
}