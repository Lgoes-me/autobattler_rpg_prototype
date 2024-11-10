using UnityEngine;

public class DialogueDataReference : IDialogue
{
    [field: SerializeField] private DialogueData Dialogue { get; set; }
}