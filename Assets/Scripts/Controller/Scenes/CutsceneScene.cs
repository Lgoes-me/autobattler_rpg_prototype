using UnityEngine;

public class CutsceneScene : MonoBehaviour
{
    [field: SerializeField] private DialogueData Dialogue { get; set; }
    [field: SerializeReference] [field: SerializeField] private GameAction EndCutsceneAction { get; set; }

    public void Init()
    {
        
        Application.Instance.GetManager<PauseManager>().PauseGame();
        
        Application.Instance.GetManager<DialogueManager>().OpenDialogue(Dialogue, () =>
        {
            EndCutsceneAction?.Invoke();
            Application.Instance.GetManager<PauseManager>().ResumeGame();
        });
    }
}