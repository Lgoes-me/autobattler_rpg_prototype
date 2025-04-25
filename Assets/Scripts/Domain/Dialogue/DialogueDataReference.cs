using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DialogueDataReference : IDialogue
{
    [field: SerializeField] private DialogueData Dialogue { get; set; }
    
    public IEnumerator ReadDialogue(DialogueManager dialogueManager, Pawn pawn)
    {
        return Dialogue.ReadDialogue(dialogueManager);
    }
}