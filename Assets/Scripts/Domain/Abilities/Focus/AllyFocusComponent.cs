
public class AllyFocusComponent : AbilityFocusComponent
{
    private FocusType FocusType { get; set; }
    private bool CanTargetSelf { get; set; }

    public AllyFocusComponent(FocusType focusType, bool canTargetSelf)
    {
        FocusType = focusType;
        CanTargetSelf = canTargetSelf;
    }

    public override PawnController ChooseFocus(PawnController pawn, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return FocusedPawn;
        }
        
        return FocusedPawn = battle.QueryAlly(pawn, FocusType, CanTargetSelf);
    }
}