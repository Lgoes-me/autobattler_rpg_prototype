using System;

public static class MapEditorExtensions
{
    public static BaseNodeView ToNodeView(this BaseNodeData node)
    {
        return node switch
        {
            SceneNodeData sceneNode => new SceneNodeView(sceneNode),
            CutsceneNodeData cutseneNode => new CutsceneNodeView(cutseneNode),
            SpawnNodeData spawnNode => new SpawnNodeView(spawnNode),
            BlockedEventNodeData blockedEventNode => new BlockedEventNodeView(blockedEventNode),
            ForkedNodeData forkedNodeData => new ForkedNodeView(forkedNodeData),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}