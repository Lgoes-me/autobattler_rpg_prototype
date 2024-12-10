using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ChangeSceneDialogueEvent : DialogueEvent
{
    [field: SerializeField] private string SceneName { get; set; }
    [field: SerializeField] private string SpawnId { get; set; }
    
    public override IEnumerator ReadDialogue(DialogueManager dialogueManager, BasePawn pawn)
    {
        Application.Instance.SceneManager.UseDoorToChangeScene(SpawnId, SceneName);
        yield return null;
    }
}