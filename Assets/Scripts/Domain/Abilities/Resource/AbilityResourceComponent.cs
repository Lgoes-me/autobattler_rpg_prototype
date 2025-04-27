public abstract class AbilityResourceComponent
{
    protected PawnController AbilityUser { get; private set; }

    protected AbilityResourceComponent(PawnController abilityUser)
    {
        AbilityUser = abilityUser;
    }

    public abstract void SpendResource();
}