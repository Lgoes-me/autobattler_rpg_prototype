
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
        return stats + Variation;
    }

    public override void ApplyStacks()
    {
        Variation += StartingVariation;
    }
}