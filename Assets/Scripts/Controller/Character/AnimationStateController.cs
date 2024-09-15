using System;
using System.Collections;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [field: SerializeField] private Animator Animator { get; set; }

    public AnimationState CurrentState { get; set; }
    private Coroutine OnAnimationEndCoroutine { get; set; }

    private void Awake()
    {
        SetAnimationState(new IdleState());
    }

    public void SetAnimationState(AnimationState state, Action callback = null)
    {
        CurrentState = state;
        Animator.applyRootMotion = true;
        Animator.Play(CurrentState.Animation);

        if (!CurrentState.Loopable)
        {
            if(OnAnimationEndCoroutine != null)
                StopCoroutine(OnAnimationEndCoroutine);
            
            OnAnimationEndCoroutine = StartCoroutine(OnAnimationComplete(callback));
        }
    }

    private IEnumerator OnAnimationComplete(Action callback)
    {
        var clipInfo = Animator.GetCurrentAnimatorClipInfo(0);
        yield return new WaitForSeconds(clipInfo[0].clip.length);
        callback?.Invoke();
    }

    private void OnCollisionEnter(Collision _)
    {
        Animator.applyRootMotion = false;
    }

    private void OnCollisionExit(Collision _)
    {
        Animator.applyRootMotion = true;
    }
    
    public void DoAnimationEvent()
    {
        CurrentState.DoAnimationEvent();
    }
}
