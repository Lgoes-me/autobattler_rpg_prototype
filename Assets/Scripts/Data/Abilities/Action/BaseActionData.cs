[System.Serializable]
public abstract class BaseActionData : AbilityComponentData
{
    public abstract AbilityActionComponent ToDomain(
        PawnController abilityUser,
        AbilityFocusComponent focusComponent,
        AbilityEffect effect);
}