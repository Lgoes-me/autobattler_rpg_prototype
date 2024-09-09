using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [field: SerializeField] private GameObject PauseCanvas { get; set; }

    private List<IPauseListener> PauseListeners { get; set; }
    
    private bool IsPaused { get; set; }

    private void Awake()
    {
        PauseListeners = new List<IPauseListener>();
        IsPaused = false;
    }

    public void SubscribePauseListener(IPauseListener pauseListener)
    {
        PauseListeners.Add(pauseListener);
    }

    public void UnsubscribePauseListener(IPauseListener pauseListener)
    {
        PauseListeners.Remove(pauseListener);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        PauseCanvas.gameObject.SetActive(true);
        IsPaused = true;

        foreach (var pauseListener in PauseListeners)
        {
            pauseListener.Pause();
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        PauseCanvas.gameObject.SetActive(false);
        IsPaused = false;

        foreach (var pauseListener in PauseListeners)
        {
            pauseListener.Resume();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!IsPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
}

public interface IPauseListener
{
    void Pause();
    void Resume();
}