using UnityEngine;

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    [field: SerializeReference] [field: SerializeField] private IDialogue Dialogue { get; set; }
}