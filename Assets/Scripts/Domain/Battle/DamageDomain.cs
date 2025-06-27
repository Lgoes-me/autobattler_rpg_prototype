using System;

public class DamageDomain
{
    public Pawn Attacker { get; }
    public int Value { get; }
    private float Multiplier { get; }
    public DamageType Type { get; }

    private DamageDomain()
    {
        Attacker = null;
        Value = 0;
        Multiplier = 1;
        Type = DamageType.Unknown;
    }
    
    public DamageDomain(Pawn attacker, float multiplier, DamageType type)
    {
        Attacker = attacker;
        Multiplier = multiplier;
        Type = type;
        Value = CalculateDamageValue();
    }

    public DamageDomain(int value)
    {
        Value = Value;
    }

    private int CalculateDamageValue()
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
    Unknown = 0,
    Physical = 1,
    Magical = 2,
}