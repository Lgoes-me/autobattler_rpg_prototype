using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScene : MonoBehaviour
{
    [field: SerializeField] private Button StartButton { get; set; }
    [field: SerializeField] private Button ContinueButton { get; set; }
    [field: SerializeField] private Button ExitButton { get; set; }

    private void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        ContinueButton.onClick.AddListener(ContinueGame);
        ExitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        
    }

    private void ContinueGame()
    {
        
    }

    private void ExitGame()
    {
        
    }
}
