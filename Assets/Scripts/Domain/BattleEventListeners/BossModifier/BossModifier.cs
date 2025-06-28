public class BossModifier : BaseIEnumerableGameEventListener
{
    public BossModifierIdentifier Identifier { get; set; }

    public BossModifier(BossModifierIdentifier identifier)
    {
        Identifier = identifier;
        Rarity = Rarity.Bronze;
    }
}

public enum BossModifierIdentifier
{
    Unknown = 0,
}