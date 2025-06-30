using Cinemachine;

public class CutsceneRoomController : BaseRoomController
{
    private DialogueData DialogueData { get; set; }
    private Transition Exit { get; set; }
    
    public CutsceneRoomController Init(CutsceneNode sceneData)
    {
        DialogueData = sceneData.DialogueData;
        Exit = sceneData.Exit;
        return this;
    }
    
    public override void SpawnPlayerAt(string spawn, CinemachineBlendDefinition blend)
    {
        Application.Instance.GetManager<DialogueManager>().OpenDialogue(DialogueData, () =>
        {
            Application.Instance.GetManager<SceneManager>().ChangeContext(Exit.Destination);
        });
    }
}