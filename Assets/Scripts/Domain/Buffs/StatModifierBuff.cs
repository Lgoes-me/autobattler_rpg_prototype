using UnityEngine;

public class StatModifierBuff : Buff
{
    private Stats Variation { get; set; }
    public StatModifierBuff(Stats variation, string id, float duration) : base(id, duration)
    {
        Variation = variation;
    }

    public virtual Stats ProcessStats(Stats stats)
    {
        return new Stats(
            stats.Strength + Variation.Strength, 
            stats.Arcane + Variation.Arcane, 
            stats.PhysicalDefence + Variation.PhysicalDefence, 
            stats.MagicalDefence + Variation.MagicalDefence);
    }
    
    public override void TryReapplyBuff()
    {
        base.TryReapplyBuff();

        Duration = Time.time;
    }
}