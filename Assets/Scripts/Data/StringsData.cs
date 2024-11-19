using System;
using UnityEngine;

[Serializable]
public class StringsData
{
    [field: SerializeField] public TextKeyData NewGameButton { get; set; }
}

[Serializable]
public class TextKeyData
{
    [field: SerializeField] private string Pt { get; set; }
    [field: SerializeField] private string En { get; set; }

    public string GetText()
    {
        var languageType = Application.Instance.ConfigManager.GetLanguage();
        
        return languageType switch
        {
            LanguageType.Pt => Pt,
            LanguageType.En => En,
            _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
        };
    }
}

public enum LanguageType
{
    Unknown,
    Pt,
    En
}