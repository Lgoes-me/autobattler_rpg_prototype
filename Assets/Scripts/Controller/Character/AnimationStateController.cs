using System;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [field: SerializeField] private Animator Animator { get; set; }
    public AnimationState CurrentState { get; private set; }

    private void Awake()
    {
        SetAnimationState(new IdleState());
    }

    public void SetAnimationState(AnimationState state)
    {
        CurrentState = state;
        Animator.applyRootMotion = true;
        Animator.Play(CurrentState.Animation);
    }

    public void DoAnimationEvent()
    {
        CurrentState.DoAnimationEvent();
    }
    
    public void CompleteAnimation()
    {
        CurrentState.DoAnimationCallback();
    }
}
