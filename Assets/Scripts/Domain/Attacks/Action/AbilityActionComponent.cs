public abstract class AbilityActionComponent
{
    protected PawnController AbilityUser { get; set; }
    protected AbilityFocusComponent FocusComponent { get; set; }
    protected AbilityEffect Effect { get; set; }

    protected AbilityActionComponent(
        PawnController abilityUser, 
        AbilityFocusComponent focusComponent, 
        AbilityEffect effect)
    {
        AbilityUser = abilityUser;
        FocusComponent = focusComponent;
        Effect = effect;
    }

    public abstract void DoAction();
}