using System.Collections.Generic;

[System.Serializable]
public class InstantActionData : ActionData
{
    public override AbilityActionComponent ToDomain(
        PawnController abilityUser,
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent)
    {
        return new InstantActionComponent(abilityUser, effects, abilityFocusComponent);
    }
}