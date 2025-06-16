using System;
using System.Collections.Generic;
using System.Linq;

public class BlockedEventNode : BaseSceneNode
{
    public string EventId { get; }
    public DialogueData Dialogue { get; }
    public List<Transition> Doors { get; set; }

    public BlockedEventNode(string eventId, DialogueData dialogue, string id, List<Transition> doors) : base(id)
    {
        EventId = eventId;
        Dialogue = dialogue;
        Doors = doors;
    }

    public override void DoTransition(Map map, Spawn spawn, Action<BaseSceneNode, Spawn> callback)
    {
        var destination = Doors.First(d => d.Start.Id != spawn.Id).Destination;
        var nextContext = map.AllNodesById[destination.NodeId];
        nextContext.DoTransition(map, destination, callback);
    }
    
    public override bool IsOpen(Map map, Spawn spawn)
    {
        return Application.Instance.GetManager<GameSaveManager>().ContainsEvent(EventId);
    }

    public override DialogueData GetDialogue(Map map, Spawn spawn)
    {
        return Dialogue;
    }
}