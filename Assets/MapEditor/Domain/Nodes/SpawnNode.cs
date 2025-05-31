using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SpawnNode : BaseSceneNode
{
    public SpawnNode(string name, string id, List<SpawnDomain> doors) : base(name, id, doors)
    {
    }

    public override void DoTransition(SpawnDomain spawn, Map map)
    {
        var nextSpawn = Doors.First();
        var nextContext = map.AllNodesById[nextSpawn.NodeId];
        nextContext.DoTransition(nextSpawn, map);
    }
}