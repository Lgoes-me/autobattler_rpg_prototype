using UnityEngine;
using UnityEngine.UI;

public class StartMenuScene : MonoBehaviour
{
    [field: SerializeField] private Button ContinueButton { get; set; }
    [field: SerializeField] private Button LoadButton { get; set; }
    [field: SerializeField] private Button StartButton { get; set; }
    [field: SerializeField] private Button SettingsButton { get; set; }
    [field: SerializeField] private Button ExitButton { get; set; }

    private void Start()
    {
        ContinueButton.onClick.AddListener(ContinueGame);
        LoadButton.onClick.AddListener(LoadGameList);
        StartButton.onClick.AddListener(StartGame);
        SettingsButton.onClick.AddListener(OpenSettings);
        ExitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        Application.Instance.GetManager<GameSaveManager>().StartNewSave();
        Application.Instance.GetManager<TimeManager>().StartClock();
        Application.Instance.GetManager<SceneManager>().StartGameIntro();
    }

    private void ContinueGame()
    {
        Application.Instance.GetManager<GameSaveManager>().LoadSave();
        
        Application.Instance.GetManager<BlessingManager>().LoadBlessings();
        Application.Instance.GetManager<TimeManager>().StartClock();
        
        var spawn =  Application.Instance.GetManager<GameSaveManager>().GetSpawn();
        Application.Instance.GetManager<SceneManager>().ChangeContext(spawn);
    }

    private void LoadGameList()
    {
    
    }
    
    private void OpenSettings()
    {
    
    }

    private void ExitGame()
    {
        UnityEngine.Application.Quit();
    }
}