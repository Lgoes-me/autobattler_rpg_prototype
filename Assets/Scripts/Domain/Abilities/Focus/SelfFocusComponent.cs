public class SelfFocusComponent : AbilityFocusComponent
{
    public SelfFocusComponent()
    {
    }

    public override PawnController ChooseFocus(PawnController pawn, Battle battle)
    {
        return pawn;
    }
}