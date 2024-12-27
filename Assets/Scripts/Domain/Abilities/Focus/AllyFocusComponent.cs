
public class AllyFocusComponent : AbilityFocusComponent
{
    private bool CanTargetSelf { get; set; }

    public AllyFocusComponent(bool canTargetSelf)
    {
        CanTargetSelf = canTargetSelf;
    }

    public override PawnController ChooseFocus(PawnController pawn, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return FocusedPawn;
        }
        
        return FocusedPawn = battle.QueryAlly(pawn, FocusType.Unknown, 0);
    }
}