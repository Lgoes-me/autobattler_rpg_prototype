using System;

public class SpawnNode : BaseSceneNode
{
    public Transition Spawn { get; }
    
    public SpawnNode(string id, Transition spawn) : base(id)
    {
        Spawn = spawn;
    }

    public override void DoTransition(Map map, Spawn spawn, Action<BaseSceneNode, Spawn> callback)
    {
        var destination = Spawn.Destination;
        var nextContext = map.AllNodesById[destination.NodeId];
        nextContext.DoTransition(map, destination, callback);
    }

    public override bool IsOpen(Map map, Spawn spawn)
    {
        return false;
    }
}