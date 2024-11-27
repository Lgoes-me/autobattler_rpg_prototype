using System;

[Serializable]
public class Stats
{
    public int Strength { get; private set; }
    public int Arcane { get; private set; }
    
    public int PhysicalDefence { get; private set; }
    public int MagicalDefence { get; private set; }
    
    public Stats(int strength, int arcane, int physicalDefence, int magicalDefence)
    {
        Strength = strength;
        Arcane = arcane;
        PhysicalDefence = physicalDefence;
        MagicalDefence = magicalDefence;
    }

    public int GetReducedDamage(DamageDomain damage)
    {
        var damageValue = damage.CalculateDamageValue();

        var reducedDamage = damage.Type switch
        {
            DamageType.Physical => damageValue - (damageValue * PhysicalDefence * 10/100),
            DamageType.Magical => damageValue - (damageValue * MagicalDefence * 10/100),
            _ => throw new ArgumentOutOfRangeException()
        };

        return reducedDamage >= 0 ? reducedDamage : 0;
    }

    public void Add(Stats stats)
    {
        Strength += stats.Strength;
        Arcane += stats.Arcane;
        PhysicalDefence += stats.PhysicalDefence;
        MagicalDefence += stats.MagicalDefence;
    }
}