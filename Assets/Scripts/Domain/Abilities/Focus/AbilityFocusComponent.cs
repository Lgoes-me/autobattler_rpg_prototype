public class AbilityFocusComponent
{
    public PawnController FocusedPawn { get; private set; }
    private TargetType Target { get; set; }
    private FocusType Focus { get; set; }
    private int Error { get; set; }

    public AbilityFocusComponent(TargetType target, FocusType focus, int error)
    {
        Target = target;
        Focus = focus;
        Error = error;
    }

    public PawnController ChooseFocus(PawnController pawn, Battle battle)
    {
        return FocusedPawn = battle.Query(pawn, Target, Focus, Error);
    }
}

public enum TargetType
{
    Self = 0,
    Ally = 1,
    Enemy = 2
}

public enum FocusType
{
    Unknown = 0,
    Closest = 1,
    Farthest = 2,
    LowestLife = 3,
    HighestLife = 4
}