using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Line : IDialogue
{
    [field: SerializeField] public string Text { get; private set; }

    public IEnumerator ReadDialogue(DialogueManager dialogueManager, PawnData pawn)
    {
        yield return dialogueManager.ShowText(this, pawn);
    }
}