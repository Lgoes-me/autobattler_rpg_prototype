using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DoGameAction: IDialogue
{
    
    [field: SerializeReference] [field: SerializeField] public GameAction GameAction { get; private set; }
    
    public IEnumerator ReadDialogue(DialogueManager dialogueManager, BasePawn pawn)
    {
        GameAction.Invoke();
        yield return null;
    }
}