public class SelfFocusComponent : AbilityFocusComponent
{
    public override PawnController ChooseFocus(PawnController user, Battle battle)
    {
        return user;
    }
}