public class EnemyFocusComponent : AbilityFocusComponent
{
    public override void ChooseFocus(PawnController user, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return;
        }

        FocusedPawn = user.Pawn.Focus;
    }
}