public class ConfigManager : IManager
{
    private Config Config { get; set; }
    private SaveManager SaveManager { get; set; }

    public void Prepare()
    {
        SaveManager = new SaveManager();
    }

    public void Init()
    {
        Config = SaveManager.LoadData<Config>();

        if (Config == null)
        {
            Config = new Config();
            SaveManager.SaveData(Config);
        }
    }

    public LanguageType GetLanguage()
    {
        return Config.Language;
    }
}