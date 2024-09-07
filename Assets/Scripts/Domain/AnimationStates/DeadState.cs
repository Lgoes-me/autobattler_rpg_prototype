public class DeadState : AnimationState
{
    public override string Animation => "Dead";
    public override bool CanBeTargeted => false;
    public override bool AbleToFight => false;
}