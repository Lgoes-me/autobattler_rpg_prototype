using System;

public class AbilityState : AnimationState
{
    public override string Animation => Ability.Animation;
    private Ability Ability { get; }
    private Action<Ability> AnimationAction { get; }
    private Action FinishCallback { get; }

    public AbilityState(Ability ability, Action<Ability> animationAction, Action finishCallback = null)
    {
        Ability = ability;
        AnimationAction = animationAction;
        FinishCallback = finishCallback;
    }

    public override void DoAnimationEvent()
    {
        base.DoAnimationEvent();
        AnimationAction?.Invoke(Ability);
    }
    
    public override void DoAnimationCallback()
    {
        base.DoAnimationEvent();
        FinishCallback?.Invoke();
    }
}