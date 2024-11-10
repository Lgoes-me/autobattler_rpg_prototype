using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOptions : IDialogue
{
    [field: SerializeField] public List<Option> Options { get; private set; }    
}


[System.Serializable]
public class Option
{
    [field: SerializeField] public string Choice { get; private set; }  
    [field: SerializeReference] [field: SerializeField] public IDialogue Response { get; private set; }  
}