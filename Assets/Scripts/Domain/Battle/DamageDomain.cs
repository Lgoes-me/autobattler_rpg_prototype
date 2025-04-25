using System;

public class DamageDomain
{
    private Pawn Attacker { get; set; }
    private float Multiplier { get; set; }
    public DamageType Type { get; private set; }

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
            DamageType.Physical => (int) Multiplier * Attacker.GetComponent<StatsComponent>().GetPawnStats().Strength,
            DamageType.Magical => (int) Multiplier * Attacker.GetComponent<StatsComponent>().GetPawnStats().Arcane,
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