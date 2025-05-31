using System.Collections.Generic;
using System.Linq;

public class Map
{
    public Dictionary<string, BaseSceneNode> AllNodesById { get; set; }

    public SceneManager SceneManager { get; private set; }

    public Map(List<BaseSceneNode> nodes, SceneManager sceneManager)
    {
        AllNodesById = nodes.ToDictionary(n => n.Id, n => n);
        SceneManager = sceneManager;
    }

    public void SpawnAt(string name)
    {
        var spawnNode = AllNodesById[name];
        var spawnDomain = spawnNode.Doors[0];

        spawnNode.DoTransition(spawnDomain, this);
    }

    public void ChangeContext(SpawnDomain spawn)
    {        
        var context = AllNodesById[spawn.DestinationNodeId];
        context.DoTransition(spawn, this);
    }
}