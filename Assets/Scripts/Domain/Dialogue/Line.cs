using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Line : IDialogue
{
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public string Info { get; private set; }

    public IEnumerator ReadDialogue(DialogueManager dialogueManager, BasePawn pawn)
    {
        yield return dialogueManager.ShowText(this, pawn);
    }
}