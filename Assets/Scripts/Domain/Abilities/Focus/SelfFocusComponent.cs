public class SelfFocusComponent : AbilityFocusComponent
{
    public override void ChooseFocus(PawnController user, Battle battle)
    {
        FocusedPawn = user;
    }
}