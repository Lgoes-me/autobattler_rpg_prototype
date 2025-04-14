public class EnemyFocusComponent : AbilityFocusComponent
{
    public override PawnController ChooseFocus(PawnController pawnController, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return FocusedPawn;
        }
        
        return FocusedPawn = battle.QueryEnemies(pawnController, pawnController.Pawn.EnemyFocusPreference);
    }
}