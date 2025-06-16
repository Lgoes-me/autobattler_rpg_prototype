using System;

public abstract class BaseSceneNode
{
    public string Id { get; }

    private BaseSceneNode()
    {
        Id = string.Empty;
    }
    
    protected BaseSceneNode(string id) : this()
    {
        Id = id;
    }

    public abstract void DoTransition(Map map, Spawn spawn, Action<BaseSceneNode, Spawn> callback);
    public abstract bool IsOpen(Map map, Spawn spawn);

    public virtual DialogueData GetDialogue(Map map, Spawn spawn)
    {
        return null;
    }
}