using System;

public class DefaultState : AnimationState
{
    public sealed override string Animation { get; set; }
    public override bool CanWalk { get; set; } = true;
    private Action FinishCallback { get; }

    public DefaultState(string animation, Action finishCallback = null)
    {
        Animation = !string.IsNullOrWhiteSpace(animation) ? animation : "Idle";
        FinishCallback = finishCallback;
    }
    
    public override void DoAnimationCallback()
    {
        base.DoAnimationEvent();
        FinishCallback?.Invoke();
    }
}