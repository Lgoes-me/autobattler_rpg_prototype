using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class AddFriendDialogueEvent : DialogueEvent
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    
    public override IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        Application.Instance.PartyManager.AddToAvailableParty(PawnData);
        yield return null;
    }
}