using UnityEngine;

public class StatModifierBuff : Buff
{
    private Stats StartingVariation { get; set; }
    private Stats Variation { get; set; }
    private bool Stackable { get; set; }
    private int Stacks { get; set; }

    public StatModifierBuff(Stats variation, string id, float duration, bool stackable = false) : base(id, duration)
    {
        StartingVariation = variation;
        Variation = variation;
        Stackable = stackable;
        Stacks = 1;
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

    public override void TryReapplyBuff()
    {
        base.TryReapplyBuff();

        Duration = Time.time;

        if (Stackable)
        {
            Variation += StartingVariation;
            Stacks++;
        }
    }
}