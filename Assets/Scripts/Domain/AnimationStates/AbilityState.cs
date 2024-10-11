using System;

public class AbilityState : AnimationState
{
    public override string Animation => Ability.Animation;
    public override bool CanTakeTurn => false;
    private Ability Ability { get; }
    private Action Callback { get; }

    public AbilityState(Ability ability, Action callback)
    {
        Ability = ability;
        Callback = callback;
    }

    public override void DoAnimationEvent()
    {
        base.DoAnimationEvent();
        Callback?.Invoke();
    }
}