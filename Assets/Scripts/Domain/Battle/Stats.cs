using System;
using System.Collections.Generic;

[Serializable]
public class Stats
{
    public Dictionary<Stat, int> StatsDictionary { get; private set; }

    public Stats(Dictionary<Stat, int> stats)
    {
        StatsDictionary = stats;
    }

    public int GetStat(Stat stat)
    {
        return StatsDictionary.TryGetValue(stat, out var value) ? value : 0;
    }

    public int GetReducedDamage(DamageDomain damage)
    {
        var damageValue = damage.Value;

        var reducedDamage = damage.Type switch
        {
            DamageType.Physical => damageValue - (damageValue * GetStat(Stat.PhysicalDefence) * 10 / 100),
            DamageType.Magical => damageValue - (damageValue * GetStat(Stat.MagicalDefence) * 10 / 100),
            DamageType.True => damageValue,
            _ => throw new ArgumentOutOfRangeException()
        };

        reducedDamage = (int) (reducedDamage + reducedDamage * GetStat(Stat.DamageModifier) / (float) 100);

        return reducedDamage >= 0 ? reducedDamage : 0;
    }

    public static Stats operator + (Stats a, Stats b)
    {
        var newStats = new Dictionary<Stat, int>();
        
        foreach (var (stat, aValue) in a.StatsDictionary)
        {
            newStats.Add(stat, aValue);
        }
        
        foreach (var (stat, bValue) in b.StatsDictionary)
        {
            if (newStats.TryGetValue(stat, out var value))
            {
                newStats[stat] = value + bValue;
                continue;
            }
            
            newStats.Add(stat, bValue);
        }
        
        return new Stats(newStats);
    }
}

public enum Stat
{
    Health,
    Mana,
    Strength,
    Arcane,
    PhysicalDefence,
    MagicalDefence,
    HealPower,
    DamageModifier,
    ManaGainModifier,
    ExperienceToLevelUp,
    Vampirismo
}