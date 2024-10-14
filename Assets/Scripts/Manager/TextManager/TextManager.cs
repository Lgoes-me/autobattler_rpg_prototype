using System.Linq;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [field: SerializeField]
    private TextAsset LanguagesFile { get; set; }
    private Strings Strings { get; set; }
    
    public void Awake()
    {
        var spreadsheetFetcher = new SpreadsheetFetcher();
        var strings = spreadsheetFetcher.FetchSpreadsheet<TextKeyDataModel>(LanguagesFile.text);
        Strings = new Strings(strings.Select(t => t.ToDomain()).ToList());
    }

    public string GetString(string textKey)
    {
        return Strings.GetText(textKey, LanguageType.Pt); //Application.ConfigManager.GameConfig.Language);
    }
}