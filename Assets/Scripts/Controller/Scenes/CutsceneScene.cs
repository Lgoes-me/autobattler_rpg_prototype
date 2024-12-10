using UnityEngine;

public class CutsceneScene : BaseScene
{
    [field: SerializeField] private DialogueData Dialogue { get; set; }

    public void Init()
    {
        Application.Instance.PauseManager.PauseGame();
        
        Application.Instance.DialogueManager.OpenDialogue(Dialogue, () =>
        {
            Application.Instance.PauseManager.ResumeGame();
        });
    }
}