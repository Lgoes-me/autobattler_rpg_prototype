using UnityEngine.Rendering;

public abstract class BaseSceneNode : BaseNode
{
    public abstract BaseRoomController Prefab { get; }
    public VolumeProfile PostProcessProfile { get; }
    public MusicType Music { get; }
    
    protected BaseSceneNode(VolumeProfile postProcessProfile, MusicType music, string id) : base(id)
    {
        PostProcessProfile = postProcessProfile;
        Music = music;
    }
}