using System;
using System.Collections.Generic;
using System.Linq;

public class BlockedEventNode : BaseSceneNode
{
    public string EventId { get; }
    public List<Transition> Doors { get; set; }

    public BlockedEventNode(string eventId, string id, List<Transition> doors) : base(id)
    {
        EventId = eventId;
        Doors = doors;
    }

    public override void DoTransition(Map map, Spawn spawn, Action<SceneData, Spawn> callback)
    {
        var destination = Doors.First(d => d.Start.Id != spawn.Id).Destination;
        var nextContext = map.AllNodesById[destination.NodeId];
        nextContext.DoTransition(map, destination, callback);
    }
    
    public override bool IsOpen(Map map, Spawn spawn)
    {
        return Application.Instance.GetManager<GameSaveManager>().ContainsEvent(EventId);
    }
}