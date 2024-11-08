
[System.Serializable]
public class Config : ISavable
{
    public string Id { get; set; }
    
    public Config()
    {
        Id = "Config.json";
    }
}