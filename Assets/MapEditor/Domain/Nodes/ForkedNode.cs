using System;
using System.Collections.Generic;

public class ForkedNode : BaseSceneNode
{
    private Transition In1 { get; }
    private Transition In2 { get; }
    private Transition Exit { get; }

    public ForkedNode(Transition in1, Transition in2, Transition exit, string id) : base(id)
    {
        In1 = in1;
        In2 = in2;
        Exit = exit;
    }

    public override void DoTransition(Map map, Spawn spawn, Action<SceneData, Spawn> callback)
    {
        var destination = spawn;
        
        if (spawn.Id == In1.Start.Id)
            destination = Exit.Destination;
        else if (spawn.Id == In2.Start.Id)
            destination = Exit.Destination;
        else if (spawn.Id == Exit.Start.Id)
            destination = In1.Destination;
        
        var nextContext = map.AllNodesById[destination.NodeId];
        
        nextContext.DoTransition(map, destination, callback);
    }
    
    public override bool IsOpen(Map map, Spawn spawn)
    {
        var destination = spawn;
        
        if (spawn.Id == In1.Start.Id)
            destination = Exit.Destination;
        else if (spawn.Id == In2.Start.Id)
            destination = Exit.Destination;
        else if (spawn.Id == Exit.Start.Id)
            destination = In1.Destination;
        
        var nextContext = map.AllNodesById[destination.NodeId];
        
        return nextContext.IsOpen(map, destination);
    }
}