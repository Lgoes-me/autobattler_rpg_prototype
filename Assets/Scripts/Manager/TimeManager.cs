using UnityEngine;

public class TimeManager : MonoBehaviour, IPauseListener
{
    [field: SerializeField] private float DuracaoDaHoraEmSegundos { get; set; }
    public float HorarioEmJogo { get; private set; }

    private float Tempo { get; set; }
    private bool IsTimePassing { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private PauseManager PauseManager { get; set; }

    public void StartClock()
    {
        PauseManager = Application.Instance.PauseManager;
        GameSaveManager = Application.Instance.GameSaveManager;

        PauseManager.SubscribePauseListener(this);

        var h = Mathf.InverseLerp(0f, 24f, GameSaveManager.GetSavedTime());
        Tempo = Mathf.Lerp(0f, DuracaoDaHoraEmSegundos * 24, h);
        IsTimePassing = true;
    }

    private void FixedUpdate()
    {
        if (!IsTimePassing)
            return;

        Tempo = (Tempo + Time.deltaTime) % (DuracaoDaHoraEmSegundos * 24);

        var t = Mathf.InverseLerp(0f, (DuracaoDaHoraEmSegundos * 24), Tempo);
        HorarioEmJogo = Mathf.Lerp(0f, 24f, t);
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