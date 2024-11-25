using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    private Config Config { get; set; }
    private SaveManager SaveManager { get; set; }

    private void Start()
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