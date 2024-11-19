
[System.Serializable]
public class Config : ISavable
{
    public string Id { get; set; }
    public LanguageType Language { get; set; }
    
    public Config()
    {
        Id = "Config.json";
        Language = LanguageType.Pt;
    }
}