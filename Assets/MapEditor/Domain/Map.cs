using System.Collections.Generic;
using System.Linq;

public class Map
{
    internal Dictionary<string, BaseSceneNode> AllNodesById { get; set; }
    internal SceneManager SceneManager { get; private set; }

    public Map(List<BaseSceneNode> nodes, SceneManager sceneManager)
    {
        AllNodesById = nodes.ToDictionary(n => n.Id, n => n);
        SceneManager = sceneManager;
    }

    public void SpawnAt(string name)
    {
        var node = AllNodesById[name];
        var spawn = node.Doors[0].Destination;

        node.DoTransition(spawn, this);
    }

    public void ChangeContext(Spawn spawn)
    {        
        var node = AllNodesById[spawn.NodeId];
        node.DoTransition(spawn, this);
    }
    
    public void ChangeContext(Transition transition)
    {
        var spawn = transition.Destination;
        var node = AllNodesById[spawn.NodeId];
        
        node.DoTransition(spawn, this);
    }
}