
public class DamageDomain
{
    public Pawn Attacker { get; }
    public int Value { get; }
    public DamageType Type { get; }

    private DamageDomain()
    {
        Attacker = null;
        Value = 0;
        Type = DamageType.Unknown;
    }
    
    public DamageDomain(Pawn attacker, int value, DamageType type) : this()
    {
        Attacker = attacker;
        Value = value;
        Type = type;
    }
}

public enum DamageType
{
    Unknown = 0,
    Physical = 1,
    Magical = 2,
    True = 3,
}