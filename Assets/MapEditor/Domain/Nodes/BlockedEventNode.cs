using System.Collections.Generic;

public class BlockedEventNode : BaseSceneNode
{
    public string EventId { get; }
    
    public BlockedEventNode(string eventId, string name, string id, List<DoorData> doors) : base(name, id, doors)
    {
        EventId = eventId;
    }
}