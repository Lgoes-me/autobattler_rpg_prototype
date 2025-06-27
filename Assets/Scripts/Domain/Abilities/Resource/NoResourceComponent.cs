public class NoResourceComponent : AbilityResourceComponent
{
    public NoResourceComponent(PawnController abilityUser) : base(abilityUser)
    {
    }
    public override void SpendResource()
    {
        var resources = AbilityUser.Pawn.GetComponent<ResourceComponent>();
        
        if (!resources.HasMana)
            return;

        resources.GainMana();
    }
}