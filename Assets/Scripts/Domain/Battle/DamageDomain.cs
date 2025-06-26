using System;

public class DamageDomain
{
    public Pawn Attacker { get; }
    private float Multiplier { get; }
    public DamageType Type { get; }

    public DamageDomain(Pawn attacker, float multiplier, DamageType type)
    {
        Attacker = attacker;
        Multiplier = multiplier;
        Type = type;
    }

    public int CalculateDamageValue()
    {
        var value = Type switch
        {
            DamageType.Physical => (int) Multiplier * Attacker.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Strength),
            DamageType.Magical => (int) Multiplier * Attacker.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Arcane),
            _ => throw new ArgumentOutOfRangeException()
        };

        return value > 0 ? value : 0;
    }
}

public enum DamageType
{
    Physical = 1,
    Magical = 2,
}