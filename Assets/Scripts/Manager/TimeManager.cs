using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour, IPauseListener, IManager
{
    [field: SerializeField] private float DuracaoDaHoraEmSegundos { get; set; }
    public float HorarioEmJogo { get; private set; }

    private float Tempo { get; set; }
    private bool IsTimePassing { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private PauseManager PauseManager { get; set; }

    public void StartClock()
    {
        PauseManager = Application.Instance.GetManager<PauseManager>();
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        
        PauseManager.SubscribePauseListener(this);

        var h = Mathf.InverseLerp(0f, 24f, GameSaveManager.GetSavedTime());
        Tempo = Mathf.Lerp(0f, DuracaoDaHoraEmSegundos * 24, h);
        IsTimePassing = true;

        StartCoroutine(ClockCoroutine());
    }

    private IEnumerator ClockCoroutine()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(1f);
            
            if (!IsTimePassing)
                continue;

            Tempo = (Tempo + 1f) % (DuracaoDaHoraEmSegundos * 24);

            var t = Mathf.InverseLerp(0f, (DuracaoDaHoraEmSegundos * 24), Tempo);
            HorarioEmJogo = Mathf.Lerp(0f, 24f, t);
        }
    }

    public void Pause()
    {
        IsTimePassing = false;
    }

    public void Resume()
    {
        IsTimePassing = true;
    }
}