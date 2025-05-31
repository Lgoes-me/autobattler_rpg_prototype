using System.Collections.Generic;
using System.Linq;

public class SpawnNode : BaseSceneNode
{
    public SpawnNode(string name, string id, List<Transition> doors) : base(name, id, doors)
    {
    }

    public override void DoTransition(Spawn spawn, Map map)
    {
        var destination = Doors.First().Destination;
        var nextContext = map.AllNodesById[destination.NodeId];
        nextContext.DoTransition(destination, map);
    }
}