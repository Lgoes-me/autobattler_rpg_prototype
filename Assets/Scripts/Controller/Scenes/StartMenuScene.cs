using UnityEngine;
using UnityEngine.UI;

public class StartMenuScene : BaseScene
{
    [field: SerializeField] private Button StartButton { get; set; }
    [field: SerializeField] private Button ContinueButton { get; set; }
    [field: SerializeField] private Button SettingsButton { get; set; }
    [field: SerializeField] private Button ExitButton { get; set; }

    private void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        ContinueButton.onClick.AddListener(ContinueGame);
        SettingsButton.onClick.AddListener(OpenSettings);
        ExitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        Application.Instance.SceneManager.StartGameIntro();
    }

    private void ContinueGame()
    {
        Application.Instance.SceneManager.StartGame();
    }

    private void OpenSettings()
    {
    
    }

    private void ExitGame()
    {
        UnityEngine.Application.Quit();
    }
}