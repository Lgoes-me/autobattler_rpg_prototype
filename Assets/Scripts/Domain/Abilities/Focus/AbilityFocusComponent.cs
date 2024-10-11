public class AbilityFocusComponent
{
    public PawnController FocusedPawn { get; set; }
    private PawnController AbilityUser { get; set; }
    private TargetType Target { get; set; }
    private FocusType Focus { get; set; }
    private int Error { get; set; }

    public AbilityFocusComponent(PawnController abilityUser, TargetType target, FocusType focus, int error)
    {
        AbilityUser = abilityUser;
        Target = target;
        Focus = focus;
        Error = error;
    }

    public PawnController ChooseFocus(Battle battle)
    {
        return FocusedPawn = battle.Query(AbilityUser, Target, Focus, Error);
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