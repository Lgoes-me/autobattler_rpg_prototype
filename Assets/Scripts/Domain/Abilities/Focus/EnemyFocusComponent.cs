public class EnemyFocusComponent : AbilityFocusComponent
{
    private FocusType FocusType { get; set; }
    private int Error { get; set; }

    public EnemyFocusComponent(FocusType focusType, int error)
    {
        FocusType = focusType;
        Error = error;
    }

    public override PawnController ChooseFocus(PawnController pawn, Battle battle)
    {
        if (FocusedPawn != null && FocusedPawn.PawnState.CanBeTargeted)
        {
            return FocusedPawn;
        }
        
        return FocusedPawn = battle.QueryEnemies(pawn, FocusType, Error);
    }
}