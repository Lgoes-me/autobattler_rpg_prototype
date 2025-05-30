using System;

public static class MapEditorExtensions
{
    public static BaseNodeView ToNodeView(this BaseNodeData node)
    {
        return node switch
        {
            SceneNodeData sceneNode => new SceneNodeView(sceneNode),
            SpawnNodeData spawnNode => new SpawnNodeView(spawnNode),
            DungeonNodeData dungeonNode => new DungeonNodeView(dungeonNode),
            BlockedEventNodeData blockedEventNode => new BlockedEventNodeView(blockedEventNode),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}