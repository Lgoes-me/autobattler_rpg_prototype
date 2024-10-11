using System.Collections.Generic;

public abstract class AbilityActionComponent
{
    protected PawnController AbilityUser { get; set; }
    protected List<AbilityEffect> Effects { get; set; }
    protected AbilityFocusComponent AbilityFocusComponent { get; set; }

    protected AbilityActionComponent(PawnController abilityUser, List<AbilityEffect> effects, AbilityFocusComponent abilityFocusComponent)
    {
        AbilityUser = abilityUser;
        Effects = effects;
        AbilityFocusComponent = abilityFocusComponent;
    }

    public abstract void DoAction();
}