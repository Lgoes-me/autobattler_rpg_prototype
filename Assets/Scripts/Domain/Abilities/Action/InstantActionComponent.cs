public class InstantActionComponent : AbilityActionComponent
{
    public InstantActionComponent(PawnController abilityUser, AbilityEffect effect) : base(abilityUser, effect)
    {
    }

    public override void DoAction()
    {
        Effect.DoAbilityEffect(AbilityUser.Ability.FocusedPawn);
    }
}