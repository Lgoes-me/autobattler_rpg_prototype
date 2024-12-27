public abstract class AbilityFocusComponent
{
    public PawnController FocusedPawn { get; protected set; }
    public abstract PawnController ChooseFocus(PawnController pawn, Battle battle);
}

public enum FocusType
{
    Unknown = 0,
    Closest = 1,
    Farthest = 2,
    LowestLife = 3,
    HighestLife = 4
}