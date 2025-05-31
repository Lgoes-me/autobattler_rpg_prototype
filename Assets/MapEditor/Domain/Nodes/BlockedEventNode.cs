using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BlockedEventNode : BaseSceneNode
{
    public string EventId { get; }
    
    public BlockedEventNode(string eventId, string name, string id, List<SpawnDomain> doors) : base(name, id, doors)
    {
        EventId = eventId;
    }

    public override void DoTransition(SpawnDomain spawn, Map map)
    {
        var nextSpawn = Doors.First(d => d.Id != spawn.Id);
        var nextContext = map.AllNodesById[nextSpawn.DestinationNodeId];
        nextContext.DoTransition(nextSpawn, map);
    }
}