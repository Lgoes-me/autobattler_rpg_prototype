public class DefaultState : AnimationState
{
    public sealed override string Animation { get; set; }
    public override bool CanWalk { get; set; } = true;

    public DefaultState(string animation)
    {
        Animation = !string.IsNullOrWhiteSpace(animation) ? animation : "Idle";
    }
}