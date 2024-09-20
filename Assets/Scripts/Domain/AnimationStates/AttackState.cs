using System;

public class AttackState : AnimationState
{
    public override string Animation => Ability.Animation;
    private Ability Ability { get; }
    private Action Callback { get; }

    public AttackState(Ability ability, Action callback)
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