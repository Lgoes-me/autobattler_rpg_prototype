using System.Collections.Generic;

[System.Serializable]
public abstract class ActionData : IComponentData
{
    public abstract AbilityActionComponent ToDomain(
        PawnController abilityUser, 
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent);
}