using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class BaseRoomController<T> : BaseRoomController where T : BaseSceneNode
{
    [field:SerializeField] private Volume PostProcessVolume { get; set; }
    
    public override BaseRoomController Init(BaseSceneNode data, Spawn spawn, CinemachineBlendDefinition blend)
        => InternalInit((T) data, spawn, blend);

    protected virtual BaseRoomController<T> InternalInit(T data, Spawn spawn, CinemachineBlendDefinition blend)
    {
        PostProcessVolume.profile = data.PostProcessProfile;
        
        Application.Instance.GetManager<AudioManager>().PlayMusic(data.Music);

        return this;
    }
}

public abstract class BaseRoomController : MonoBehaviour
{
    public abstract BaseRoomController Init(BaseSceneNode data, Spawn spawn, CinemachineBlendDefinition blend);
}
