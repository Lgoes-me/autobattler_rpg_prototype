using System;

public class DamageDomain
{
    public PawnDomain Attacker { get; private set; }
    public float Multiplier { get; private set; }
    public DamageType Type { get; private set; }

    public DamageDomain(PawnDomain attacker, float multiplier, DamageType type)
    {
        Attacker = attacker;
        Multiplier = multiplier;
        Type = type;
    }

    public int CalculateDamageValue()
    {
        return Type switch
        {
            DamageType.Slash => (int) Multiplier * Attacker.Stats.Strength,
            DamageType.Magical => (int) Multiplier * Attacker.Stats.Arcane,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}