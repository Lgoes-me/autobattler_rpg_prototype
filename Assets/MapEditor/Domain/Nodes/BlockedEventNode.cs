using System;
using System.Collections.Generic;
using System.Linq;

public class BlockedEventNode : BaseSceneNode
{
    public string EventId { get; }
    
    public BlockedEventNode(string eventId, string id, List<Transition> doors) : base(id, doors)
    {
        EventId = eventId;
    }

    public override void DoTransition(Map map, Spawn spawn, Action<SceneNode, Spawn> callback)
    {
        var destination = Doors.First(d => d.Start.Id != spawn.Id).Destination;
        var nextContext = map.AllNodesById[destination.NodeId];
        nextContext.DoTransition(map, destination, callback);
    }
    
    public override bool IsOpen()
    {
        return true;
    }
}