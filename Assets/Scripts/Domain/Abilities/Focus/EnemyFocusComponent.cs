public class EnemyFocusComponent : AbilityFocusComponent
{
    public override PawnController ChooseFocus(PawnController user, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return FocusedPawn;
        }
        
        return FocusedPawn = battle.QueryEnemies(user, user.Pawn.EnemyFocusPreference);
    }
}