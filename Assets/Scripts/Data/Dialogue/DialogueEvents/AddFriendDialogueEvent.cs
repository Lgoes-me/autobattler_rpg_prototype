using UnityEngine;

[System.Serializable]
public class AddFriendDialogueEvent : DialogueEvent
{
    [field: SerializeField] private PawnData PawnData { get; set; }
}