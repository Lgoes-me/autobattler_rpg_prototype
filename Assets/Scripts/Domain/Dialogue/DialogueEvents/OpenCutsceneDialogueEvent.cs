using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class OpenCutsceneDialogueEvent : DialogueEvent
{
    [field: SerializeField] private string SceneName { get; set; }

    public override IEnumerator ReadDialogue(DialogueManager dialogueManager, BasePawn pawn)
    {
        Application.Instance.SceneManager.OpenCutscene(SceneName);
        yield return null;
    }
}