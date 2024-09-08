using System;

public class SpecialAttackState : AnimationState
{
    public override string Animation => Attack.Animation;
    public override bool CanWalk => true;
    private Attack Attack { get; }
    private Action Callback { get; }

    public SpecialAttackState(Attack attack, Action callback)
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