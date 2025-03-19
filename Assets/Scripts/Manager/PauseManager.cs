using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [field: SerializeField] private GameObject PauseCanvas { get; set; }

    private List<IPauseListener> PauseListeners { get; set; }
    
    private bool IsPaused { get; set; }
    private bool CanResume { get; set; }

    private void Awake()
    {
        PauseListeners = new List<IPauseListener>();
        IsPaused = false;
        CanResume = false;
    }

    public void SubscribePauseListener(IPauseListener pauseListener)
    {
        PauseListeners.Add(pauseListener);
    }

    public void UnsubscribePauseListener(IPauseListener pauseListener)
    {
        PauseListeners.Remove(pauseListener);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        IsPaused = true;

        foreach (var pauseListener in PauseListeners)
        {
            pauseListener.Pause();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsPaused = false;
        CanResume = false;

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
                PauseCanvas.gameObject.SetActive(true);
                CanResume = true;
                PauseGame();
            }
            else if (CanResume)
            {
                PauseCanvas.gameObject.SetActive(false);
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