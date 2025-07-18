public class Blessing : BaseIEnumerableGameEventListener
{
    public BlessingIdentifier Identifier { get; }

    public Blessing(BlessingIdentifier identifier)
    {
        Identifier = identifier;
        Rarity = Rarity.Common;
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
    BonusDeStatQuandoRecebeDano = 02_06,
    
    CuraBaseadaNoDanoCausado = 03_01,
    TransferirComoDanoEmAreaACuraRecebida = 03_02,
    TransferirComoDanoEmAreaODanoRecebido = 03_03,
    ReduzODanoRecebido = 03_04,
    AumentaODanoCausado = 03_05,
    
    RegeneracaoDeMana = 04_01,
    ManaNoInicioDaLuta = 04_02,
    DanoRecebidoViraMana = 04_03,
    DobraAQuantidadeDeManaRecebida = 04_04,
    BonusDeStatQuandoGanhaMana = 04_05,
    
    MelhoraASorte = 05_01,
    AumentaAQuantidadeDeEscolhas = 05_02,
    RemoveBonusNegativos = 05_03,
    ChanceDeEco = 05_04,
    ChanceDeReceberPremioSecundario = 05_05,
    
    OpcoesExtrasNosPremios = 06_01,
}