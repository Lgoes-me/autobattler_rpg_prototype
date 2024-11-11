using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class DialogueData : ScriptableObject, IDialogue
{
    [field: SerializeReference] [field: SerializeField] private IDialogue Dialogue { get; set; }

    public IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        yield return Dialogue.ReadDialogue(dialogueManager);
    }
}