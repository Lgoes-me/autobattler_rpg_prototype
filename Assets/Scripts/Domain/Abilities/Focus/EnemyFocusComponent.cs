public class EnemyFocusComponent : AbilityFocusComponent
{
    private int Error { get; set; }

    public EnemyFocusComponent(int error)
    {
        Error = error;
    }

    public override PawnController ChooseFocus(PawnController pawnController, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return FocusedPawn;
        }
        
        return FocusedPawn = battle.QueryEnemies(pawnController, pawnController.Pawn.EnemyFocusPreference, Error);
    }
}