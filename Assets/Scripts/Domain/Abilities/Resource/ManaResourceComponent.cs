public class ManaResourceComponent : AbilityResourceComponent
{
    private int ManaCost { get; set; }

    public ManaResourceComponent(PawnController abilityUser, int manaCost) : base(abilityUser)
    {
        ManaCost = manaCost;
    }

    public override bool HasResource()
    {
        var stats = AbilityUser.Pawn.GetComponent<StatsComponent>();
        return stats.Mana >= ManaCost;
    }

    public override void SpendResource()
    {
        var stats = AbilityUser.Pawn.GetComponent<StatsComponent>();

        if (!stats.HasMana)
            return;

        stats.SpentMana(ManaCost);
    }
}