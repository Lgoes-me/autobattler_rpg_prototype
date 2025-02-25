using System;

public static class EditorExtensions
{
    public static BaseNodeView ToNodeView(this BaseNodeData node)
    {
        return node switch
        {
            SceneNodeData sceneNode => new SceneNodeView(sceneNode),
            SpawnNodeData spawnNode => new SpawnNodeView(spawnNode),
            DungeonNodeData dungeonNode => new DungeonNodeView(dungeonNode),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
        
}