public class Blessing : BaseIEnumerableGameEventListener
{
    public BlessingIdentifier Identifier { get; set; }

    public Blessing(BlessingIdentifier identifier)
    {
        Identifier = identifier;
        Rarity = Rarity.Unknown;
    }
}

public enum BlessingIdentifier
{
    BattleStartGainMana,
    OnAttackHeal,
    DamageEnemiesOnEnemyDeath
}