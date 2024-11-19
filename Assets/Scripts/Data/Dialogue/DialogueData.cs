using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class DialogueData : ScriptableObject, IDialogue
{
    [field: SerializeField] private List<DialogueTranslations> DialogueVariation { get; set; }

    public IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        yield return DialogueVariation
            .First(d => d.Language == Application.Instance.ConfigManager.GetLanguage())
            .Dialogue
            .ReadDialogue(dialogueManager);
    }
}

[Serializable]
public class DialogueTranslations
{
    [field: SerializeField] public LanguageType Language { get; private set; }
    [field: SerializeReference] [field: SerializeField] public IDialogue Dialogue { get; private set; }
}