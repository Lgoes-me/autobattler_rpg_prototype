public class DeadState : AnimationState
{
    public override string Animation => "Dead";
    public override bool CanBeTargeted => false;
    public override bool AbleToFight => false;
    public override bool WillRevive => _willRevive;
    
    private readonly bool _willRevive;
    
    public DeadState(bool willRevive)
    {
        _willRevive = willRevive;
    }
}