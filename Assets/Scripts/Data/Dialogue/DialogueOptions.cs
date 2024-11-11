using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueOptions : IDialogue
{
    [field: SerializeField] private List<Option> Options { get; set; }

    public IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        yield return dialogueManager.ShowOptions(Options);
    }
}


[Serializable]
public class Option
{
    [field: SerializeField] public string Choice { get; private set; }  
    [field: SerializeReference] [field: SerializeField] private IDialogue Response { get; set; }  
    
    public IEnumerator ChooseOption(DialogueManager dialogueManager)
    {
        yield return Response.ReadDialogue(dialogueManager);
    }
}