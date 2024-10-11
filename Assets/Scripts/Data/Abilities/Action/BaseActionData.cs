using System.Collections.Generic;

[System.Serializable]
public abstract class BaseActionData : BaseComponentData
{
    public abstract AbilityActionComponent ToDomain(
        PawnController abilityUser, 
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent);
}