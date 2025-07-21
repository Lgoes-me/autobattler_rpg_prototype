public class DeadState : AnimationState
{
    public override string Animation => "Dead";
    public override bool CanBeTargeted => false;
    public override bool AbleToFight => WillRevive;
    
    private bool WillRevive { get; }

    public DeadState(bool willRevive)
    {
        WillRevive = willRevive;
    }
}