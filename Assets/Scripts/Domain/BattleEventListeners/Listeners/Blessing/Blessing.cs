public class Blessing : BaseIEnumerableGameEventListener
{
    public BlessingIdentifier Identifier { get; set; }

    public Blessing(BlessingIdentifier identifier)
    {
        Identifier = identifier;
        Rarity = Rarity.Bronze;
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
    AumentaCuraPercentualmente = 01_08,
    BonusDeStatQuandoCuraAcontece = 01_09,
    
    RevivePrimeiroAliadoAMorrerEmCombate = 02_01,
    ReviveTodosAliadosAMorreremEmCombate = 02_02,
    DanoEmAreaQuandoAliadoMorre = 02_03,
    DanoEmAreaQuandoInimigoMorre = 02_04,
    DanoDeVingançaNoInimigoQuandoAliadoMorre = 02_05,
    
    
    
    BattleStartGainMana = 0,
}