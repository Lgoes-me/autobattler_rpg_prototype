using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Map
{
    public Dictionary<string, BaseSceneNode> AllNodesById { get; set; }
    public Dictionary<string, SceneNode> SceneNodeById { get; set; }
    public Dictionary<string, SpawnNode> SpawnsByName { get; set; }

    public BaseSceneNode CurrentContext { get; set; }
    private SceneManager SceneManager { get; set; }

    public Map(List<BaseSceneNode> nodes, SceneManager sceneManager)
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
        var spawnDomain = SpawnsByName[name].Doors[0].ToDomain(true);
        var newContext = AllNodesById[spawnDomain.SceneId];

        await EnterCurrentContext(newContext, spawnDomain);
    }

    public async void ChangeContext(SpawnDomain spawn)
    {
        if (CurrentContext is DungeonNode dungeonNodeData)
        {
            if (AllNodesById.TryGetValue(spawn.SceneId, out var node))
            {
                dungeonNodeData.Clear();
                await EnterCurrentContext(node, spawn);
            }
            else
            {
                await EnterCurrentContext(CurrentContext, spawn);
            }
        }
        else
        {
            var newContext = AllNodesById[spawn.SceneId];
            await EnterCurrentContext(newContext, spawn);
        }
    }

    private async Task EnterCurrentContext(BaseSceneNode nextContext, SpawnDomain spawn)
    {
        CurrentContext = nextContext;

        switch (CurrentContext)
        {
            case DungeonNode dungeonNode:
                await SceneManager.LoadNewRoom();
                dungeonNode.DoTransition(spawn, SceneManager.EnterRoom);
                break;

            case SceneNode sceneNode:
                await SceneManager.LoadNewRoom();
                SceneManager.EnterRoom(sceneNode, spawn);
                break;

            case SpawnNode spawnNode:
                var nextSpawn = spawnNode.Doors[0].ToDomain(true);
                var nextNode = AllNodesById[nextSpawn.SceneId];
                await EnterCurrentContext(nextNode, nextSpawn);
                break;
        }
    }
}