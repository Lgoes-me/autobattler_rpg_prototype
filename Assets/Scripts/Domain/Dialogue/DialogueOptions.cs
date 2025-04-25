using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueOptions : IDialogue
{
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public List<Option> Options { get; private set; }

    public IEnumerator ReadDialogue(DialogueManager dialogueManager, Pawn pawn)
    {
        yield return dialogueManager.ShowOptions(this, pawn);
    }
}


[Serializable]
public class Option
{
    [field: SerializeField] public string Choice { get; private set; }  
    [field: SerializeReference] [field: SerializeField] private IDialogue Response { get; set; }  
    
    public IEnumerator ChooseOption(DialogueManager dialogueManager, Pawn pawn)
    {
        yield return Response.ReadDialogue(dialogueManager, pawn);
    }
}