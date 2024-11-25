using UnityEngine;

public class CutsceneScene : BaseScene
{
    [field: SerializeField] private DialogueData Dialogue { get; set; }

    [field: SerializeField] private string SceneName { get; set; }
    [field: SerializeField] private string DoorName { get; set; }

    public void Init()
    {
        Application.Instance.DialogueManager.OpenDialogue(Dialogue,
            () => { Application.Instance.SceneManager.UseDoorToChangeScene(new SpawnDomain(DoorName, SceneName)); });
    }
}