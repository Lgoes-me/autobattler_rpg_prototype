using Cinemachine;
using UnityEngine;

public abstract class BaseRoomController<T> : BaseRoomController where T : BaseSceneNode
{
    public override BaseRoomController Init(BaseSceneNode data, Spawn spawn, CinemachineBlendDefinition blend)
        => InternalInit((T) data, spawn, blend);
    
    protected abstract BaseRoomController<T> InternalInit(T data, Spawn spawn, CinemachineBlendDefinition blend);
}

public abstract class BaseRoomController : MonoBehaviour
{
    public abstract BaseRoomController Init(BaseSceneNode data, Spawn spawn, CinemachineBlendDefinition blend);
}
