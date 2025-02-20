using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SceneGraph
{
    public Dictionary<string, BaseSceneNode> AllNodesById { get; set; }
    public Dictionary<string, SceneNode> SceneNodeById { get; set; }
    public Dictionary<string, SpawnNode> SpawnsByName { get; set; }

    public BaseSceneNode CurrentContext { get; set; }
    private SceneManager SceneManager { get; set; }

    public SceneGraph(List<BaseSceneNode> nodes, SceneManager sceneManager)
    {
        AllNodesById = nodes.ToDictionary(n => n.Id, n => n);

        SceneNodeById = nodes
            .Where(n => n is SceneNode)
            .ToDictionary(n => n.Id, n => n as SceneNode);

        SpawnsByName = nodes
            .Where(n => n is SpawnNode)
            .ToDictionary(n => n.Name, n => n as SpawnNode);

        SceneManager = sceneManager;
    }

    public async void SpawnAt(string name)
    {
        var spawnDomain = SpawnsByName[name].Doors[0].ToDomain();
        var newContext = AllNodesById[spawnDomain.SceneId];

        await EnterCurrentContext(newContext, spawnDomain);
    }

    public async void ChangeContext(SpawnDomain spawn)
    {
        var newContext = AllNodesById[spawn.SceneId];
        
        if (CurrentContext is DungeonNode dungeonNodeData && CurrentContext != newContext)
        {
            dungeonNodeData.Clear();
        }
        
        await EnterCurrentContext(newContext, spawn);
    }

    private async Task EnterCurrentContext(BaseSceneNode nextContext, SpawnDomain spawn)
    {
        CurrentContext = nextContext;

        switch (CurrentContext)
        {
            case DungeonNode dungeonNode:
                await SceneManager.LoadNewRoom();
                SceneManager.EnterRoom(dungeonNode.GetRoom(spawn), spawn);
                break;

            case GameEventNode gameEventNode:
                gameEventNode.DoAction();
                break;

            case SceneNode sceneNode:
                await SceneManager.LoadNewRoom();
                SceneManager.EnterRoom(sceneNode, spawn);
                break;

            case SpawnNode spawnNode:
                var nextSpawn = spawnNode.Doors[0].ToDomain();
                var nextNode = AllNodesById[nextSpawn.SceneId];
                await EnterCurrentContext(nextNode, nextSpawn);
                break;
        }
    }
}