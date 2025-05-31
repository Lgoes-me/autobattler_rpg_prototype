using System.Collections.Generic;
using System.Linq;

public class BlockedEventNode : BaseSceneNode
{
    public string EventId { get; }
    
    public BlockedEventNode(string eventId, string name, string id, List<Transition> doors) : base(name, id, doors)
    {
        EventId = eventId;
    }

    public override void DoTransition(Spawn spawn, Map map)
    {
        var destination = Doors.First(d => d.Start.Id != spawn.Id).Destination;
        var nextContext = map.AllNodesById[destination.NodeId];
        nextContext.DoTransition(destination, map);
    }
}