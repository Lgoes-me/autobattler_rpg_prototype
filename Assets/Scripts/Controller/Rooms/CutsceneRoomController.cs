using Cinemachine;

public class CutsceneRoomController : BaseRoomController<CutsceneNode>
{
    private DialogueData DialogueData { get; set; }
    private Transition Exit { get; set; }
    
    protected override BaseRoomController<CutsceneNode> InternalInit(CutsceneNode data, Spawn spawn, CinemachineBlendDefinition blend)
    {
        DialogueData = data.DialogueData;
        Exit = data.Exit;
        
        Application.Instance.GetManager<GameSaveManager>().SetSpawn(spawn);
        Application.Instance.GetManager<AudioManager>().PlayMusic(data.Music);
        
        StartCutscene();
        return this;
    }
    
    private void StartCutscene()
    {
        Application.Instance.GetManager<DialogueManager>().OpenDialogue(DialogueData, () =>
        {
            Application.Instance.GetManager<SceneManager>().ChangeContext(Exit.Destination);
        });
    }
}