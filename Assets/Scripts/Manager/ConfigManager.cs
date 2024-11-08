using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    [field: SerializeField] private SaveManager SaveManager { get; set; }
    
    private Config Config { get; set; }

    public void Init()
    {
        Config = SaveManager.LoadData<Config>() ?? new Config();
    }
}