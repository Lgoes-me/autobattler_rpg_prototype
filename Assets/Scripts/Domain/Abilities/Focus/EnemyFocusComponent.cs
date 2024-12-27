public class EnemyFocusComponent : AbilityFocusComponent
{
    private int Error { get; set; }

    public EnemyFocusComponent(int error)
    {
        Error = error;
    }

    public override PawnController ChooseFocus(PawnController pawn, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return FocusedPawn;
        }
        
        return FocusedPawn = battle.QueryEnemies(pawn, FocusType.Unknown, Error);
    }
}