using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Line : IDialogue
{
    [field: SerializeField] private string Text { get; set; }

    public IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        yield return dialogueManager.ShowText(Text);
    }
}