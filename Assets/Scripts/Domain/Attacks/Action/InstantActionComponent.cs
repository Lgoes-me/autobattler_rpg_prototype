public class InstantActionComponent : AbilityActionComponent
{
    public InstantActionComponent(
        PawnController abilityUser, 
        AbilityFocusComponent focusComponent,
        AbilityEffect effect) 
        : base(abilityUser, focusComponent, effect)
    {
        
    }

    public override void DoAction()
    {
        Effect.DoAbilityEffect(FocusComponent.FocusedPawns);
    }
}