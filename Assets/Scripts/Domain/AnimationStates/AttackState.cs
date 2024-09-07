using System;

public class AttackState : AnimationState
{
    public override string Animation => Attack.Animation;
    private Attack Attack { get; }
    private Action Callback { get; }

    public AttackState(Attack attack, Action callback)
    {
        Attack = attack;
        Callback = callback;
    }

    public override void DoAnimationEvent()
    {
        base.DoAnimationEvent();
        Callback?.Invoke();
    }
}