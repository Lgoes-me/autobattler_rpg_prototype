using System;
using System.Collections;
using UnityEngine;

public class AnimationStateMachine : MonoBehaviour
{
    [field: SerializeField] private Animator Animator { get; set; }

    private AnimationState CurrentState { get; set; }
    private Coroutine OnAnimationEndCoroutine { get; set; }

    private void Awake()
    {
        SetAnimationState(new IdleState());
    }

    public void SetAnimationState(AnimationState state)
    {
        CurrentState = state;
        Animator.applyRootMotion = true;
        
        if (CurrentState.CanAttack)
        {
            Animator.SetLayerWeight(1, CurrentState.CanAttack ? 1 : 0);
        }

        Animator.CrossFade(CurrentState.Animation, 0.3f);

        if (CurrentState is not IdleState)
        {
            if(OnAnimationEndCoroutine != null)
                StopCoroutine(OnAnimationEndCoroutine);
            
            OnAnimationEndCoroutine = StartCoroutine(OnAnimationComplete());
        }
    }

    private IEnumerator OnAnimationComplete()
    {
        var clipInfo = Animator.GetCurrentAnimatorClipInfo(0)[0];
        yield return new WaitForSeconds(clipInfo.clip.length - 0.3f);
        SetAnimationState(new IdleState());
    }

    private void OnCollisionEnter(Collision _)
    {
        Animator.applyRootMotion = false;
    }

    private void OnCollisionExit(Collision _)
    {
        Animator.applyRootMotion = true;
    }
}
