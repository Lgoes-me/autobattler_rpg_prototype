
[System.Serializable]
public class Config : ISavable
{
    public Metadata Metadata { get; set; }
    public LanguageType Language { get; set; }
    
    public Config()
    {
        Metadata = new Metadata().CreateNewStaticFile("Config", ".json");
        Language = LanguageType.Pt;
    }

}