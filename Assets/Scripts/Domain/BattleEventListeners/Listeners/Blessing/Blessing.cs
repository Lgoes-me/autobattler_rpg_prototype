public class Blessing : BaseIEnumerableGameEventListener
{
    public BlessingIdentifier Identifier { get; set; }

    public Blessing(BlessingIdentifier identifier)
    {
        Identifier = identifier;
    }
}

public enum BlessingIdentifier
{
    Unknown,
    BattleStartGainMana,
    OnAttackHeal,
}