public class DanceState : AnimationState
{
    public override string Animation => "Dance";
    public override bool CanTakeTurn => false;
    public override bool AbleToFight => false;
}