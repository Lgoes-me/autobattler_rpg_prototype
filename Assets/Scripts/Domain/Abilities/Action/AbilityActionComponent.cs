public abstract class AbilityActionComponent
{
    protected PawnController AbilityUser { get; set; }
    protected AbilityEffect Effect { get; set; }

    protected AbilityActionComponent(PawnController abilityUser, AbilityEffect effect)
    {
        AbilityUser = abilityUser;
        Effect = effect;
    }

    public abstract void DoAction();
}