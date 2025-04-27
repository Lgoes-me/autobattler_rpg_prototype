public class NoResourceComponent : AbilityResourceComponent
{
    public NoResourceComponent(PawnController abilityUser) : base(abilityUser)
    {
    }
    public override void SpendResource()
    {
        var stats = AbilityUser.Pawn.GetComponent<StatsComponent>();

        if (!stats.HasMana)
            return;

        stats.GainMana();
    }
}