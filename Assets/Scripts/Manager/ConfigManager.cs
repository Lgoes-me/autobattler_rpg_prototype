public class ConfigManager
{
    private Config Config { get; set; }
    private SaveManager SaveManager { get; set; }

    public void Prepare()
    {
        SaveManager = Application.Instance.SaveManager;
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