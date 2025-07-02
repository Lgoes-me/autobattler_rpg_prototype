using Cinemachine;

public class CutsceneRoomController : BaseRoomController<CutsceneNode>
{
    protected override BaseRoomController<CutsceneNode> InternalInit(CutsceneNode data, Spawn spawn, CinemachineBlendDefinition blend)
    {
        base.InternalInit(data, spawn, blend);
        
        Application.Instance.GetManager<DialogueManager>().OpenDialogue(data.DialogueData, () =>
        {
            Application.Instance.GetManager<SceneManager>().ChangeContext(data.Exit.Destination);
        });
        
        return this;
    }
}