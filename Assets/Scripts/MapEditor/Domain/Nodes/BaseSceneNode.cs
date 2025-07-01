public abstract class BaseSceneNode : BaseNode
{
    public abstract BaseRoomController Prefab { get; }
    
    protected BaseSceneNode(string id) : base(id)
    {
        
    }
}