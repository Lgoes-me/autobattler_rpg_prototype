using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DialogueDataReference : IDialogue
{
    [field: SerializeField] private DialogueData Dialogue { get; set; }
    
    public IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        return Dialogue.ReadDialogue(dialogueManager);
    }
}