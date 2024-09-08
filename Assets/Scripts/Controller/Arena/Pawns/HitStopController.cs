using System.Collections;
using UnityEngine;

public class HitStopController : MonoBehaviour, IPauseListener
{
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private SpriteRenderer Sprite { get; set; }
    
    private bool Stopped { get; set; }
    private bool Paused { get; set; }

    private void Start()
    {
        Application.Instance.PauseManager.SubscribePauseListener(this);
    }

    public void HitStop(float percentage, float time, bool isGlobal)
    {
        if(Stopped) return;

        if (isGlobal)
        {
            Time.timeScale = percentage;
        }
        
        Animator.speed = percentage;
        Sprite.color = Color.red;
        
        StartCoroutine(HitStopCotoutine(time, isGlobal));
    }

    private IEnumerator HitStopCotoutine(float time, bool isGlobal)
    {
        yield return new WaitForSecondsRealtime(time);
        yield return new WaitWhile(() => Paused);
        
        if (isGlobal)
        {
            Time.timeScale = 1;
        }
        
        Animator.speed = 1;
        Sprite.color = Color.white;
    }

    public void Pause()
    {
        Paused = true;
    }

    public void Resume()
    {
        Paused = false;
    }

    private void OnDestroy()
    {
        Application.Instance.PauseManager.UnsubscribePauseListener(this);
    }
}
