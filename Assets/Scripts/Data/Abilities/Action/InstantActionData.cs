[System.Serializable]
public class InstantActionData : BaseActionData
{
    public override AbilityActionComponent ToDomain(
        PawnController abilityUser,
        AbilityFocusComponent focusComponent,
        AbilityEffect effect)
    {
        return new InstantActionComponent(abilityUser, focusComponent, effect);
    }
}