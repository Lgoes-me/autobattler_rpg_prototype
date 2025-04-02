using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopController : MonoBehaviour, IPauseListener
{
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private List<SpriteRenderer> Sprites { get; set; }
    
    private bool Stopped { get; set; }
    private bool Paused { get; set; }

    private void Start()
    {
        Application.Instance.GetManager<PauseManager>().SubscribePauseListener(this);
    }

    public void HitStop(float percentage, float time, bool isGlobal, Color color)
    {
        if (Stopped) return;

        Stopped = true;
        
        if (isGlobal)
        {
            Time.timeScale = percentage;
        }
        
        Animator.speed = percentage;

        foreach (var sprite in Sprites)
        {
            sprite.color = color;
        }
        
        StartCoroutine(HitStopCoroutine(time, isGlobal));
    }

    private IEnumerator HitStopCoroutine(float time, bool isGlobal)
    {
        yield return new WaitForSecondsRealtime(time);
        yield return new WaitWhile(() => Paused);
        
        if (isGlobal)
        {
            Time.timeScale = 1;
        }
        
        Animator.speed = 1;
        
        foreach (var sprite in Sprites)
        {
            sprite.color = Color.white;
        }

        Stopped = false;
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
        Application.Instance.GetManager<PauseManager>().UnsubscribePauseListener(this);
    }
}
