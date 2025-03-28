using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    [field: SerializeField] private PawnData Pawn { get; set; }
    [field: SerializeField] private List<DialogueTranslations> DialogueVariation { get; set; }

    public IEnumerator ReadDialogue(DialogueManager dialogueManager)
    {
        var pawn = Pawn.ToBaseDomain();
        
        yield return DialogueVariation
            .First(d => d.Language == Application.Instance.GetManager<ConfigManager>().GetLanguage())
            .Dialogue
            .ReadDialogue(dialogueManager, pawn);
    }
}

[Serializable]
public class DialogueTranslations
{
    [field: SerializeField] public LanguageType Language { get; private set; }
    [field: SerializeReference] [field: SerializeField] public IDialogue Dialogue { get; private set; }
}