using System.Collections.Generic;

public class InstantActionComponent : AbilityActionComponent
{
    public InstantActionComponent(
        PawnController abilityUser, 
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent) : base(abilityUser, effects, abilityFocusComponent)
    {
    }

    public override void DoAction()
    {
        foreach (var effect in Effects)
        {
            effect.DoAbilityEffect(AbilityFocusComponent.FocusedPawn);
        }
    }
}