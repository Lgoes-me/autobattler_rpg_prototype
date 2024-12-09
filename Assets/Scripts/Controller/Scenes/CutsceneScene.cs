using UnityEngine;

public class CutsceneScene : BaseScene
{
    [field: SerializeField] private DialogueData Dialogue { get; set; }

    [field: SerializeField] private string SceneName { get; set; }
    [field: SerializeField] private string DoorName { get; set; }

    public void Init()
    {
        Application.Instance.PauseManager.PauseGame();
        Application.Instance.DialogueManager.OpenDialogue(Dialogue, GoToNextScene);
    }

    private void GoToNextScene()
    {
        Application.Instance.PauseManager.ResumeGame();
        Application.Instance.SceneManager.UseDoorToChangeScene(DoorName, SceneName);
    }
}