public class Blessing : BaseIEnumerableGameEventListener
{
    public BlessingIdentifier Identifier { get; set; }

    public Blessing(BlessingIdentifier identifier, Rarity rarity)
    {
        Identifier = identifier;
        Rarity = rarity;
    }
}

public enum BlessingIdentifier
{
    CuraInicioDaLuta = 01_01,
    CuraFimDaLuta = 01_02,
    CuraAoAtacar = 01_03,
    CuraAoGastarMana = 01_04,
    CuraQuandoAliadoMorre = 01_05,
    CuraQuandoInimigoMorre = 01_06,
    CuraAoLongoDoTempo = 01_07,
    CuraAumentadaPercentualmente = 01_08,
    
    
    BonusDeStatQuandoCuraAcontece,
    
    
    BattleStartGainMana = 0,
    OnAttackHeal = 1,
    DamageEnemiesOnEnemyDeath = 2
}