
public class StatModifierBuff : BuffComponent
{
    private Stats StartingVariation { get; set; }
    private Stats Variation { get; set; }

    public StatModifierBuff(Stats variation)
    {
        StartingVariation = variation;
        Variation = variation;
    }

    public Stats ProcessStats(Stats stats)
    {
        return new Stats(
            stats.Health,
            stats.Mana,
            stats.Strength + Variation.Strength,
            stats.Arcane + Variation.Arcane,
            stats.PhysicalDefence + Variation.PhysicalDefence,
            stats.MagicalDefence + Variation.MagicalDefence);
    }


    public override void ApplyStacks()
    {
        Variation += StartingVariation;
    }
}