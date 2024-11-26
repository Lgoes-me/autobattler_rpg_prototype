using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue : IDialogue
{
    [field: SerializeReference] [field: SerializeField] private List<IDialogue> Lines { get; set; }
    
    public IEnumerator ReadDialogue(DialogueManager dialogueManager, PawnData pawn)
    {
        foreach (var line in Lines)
        {
            yield return line.ReadDialogue(dialogueManager, pawn);
        }
    }
}