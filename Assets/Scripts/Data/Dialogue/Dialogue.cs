using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : IDialogue
{
    [field: SerializeReference] [field: SerializeField] private List<IDialogue> Lines { get; set; }
    
    public IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        foreach (var line in Lines)
        {
            yield return line.ReadDialogue(dialogueManager);
        }
    }
}