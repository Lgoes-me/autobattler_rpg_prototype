using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkippableDialogue : IDialogue
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeReference] [field: SerializeField] private List<IDialogue> Lines { get; set; }
    
    public IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        if (Application.Instance.GameSaveManager.HasReadDialogue(Id))
        {
            yield return null;
        }
        else
        {
            foreach (var line in Lines)
            {
                yield return line.ReadDialogue(dialogueManager);
            }
            
            Application.Instance.GameSaveManager.SaveDialogue(this);
        }
    }
}