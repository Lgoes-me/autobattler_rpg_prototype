
public class AllyFocusComponent : AbilityFocusComponent
{
    private bool CanTargetSelf { get; set; }

    public AllyFocusComponent(bool canTargetSelf)
    {
        CanTargetSelf = canTargetSelf;
    }

    public override void ChooseFocus(PawnController user, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return;
        }
        
        FocusedPawn = battle.QueryAlly(user, user.Pawn.GetComponent<FocusComponent>().AllyFocusPreference, CanTargetSelf);
    }
}