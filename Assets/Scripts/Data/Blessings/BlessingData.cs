using System;
using UnityEngine;

[CreateAssetMenu]
public class BlessingData : ScriptableObject
{
    [field: SerializeField] public BlessingIdentifier Id { get; private set; }
    [field: SerializeField] public Rarity Rarity { get; private set; }
    
    [field: SerializeField] [field: SerializeReference] private BaseBattleEventListenerData[] Events { get; set; }
    
    public void DoBattleStartEvent(Battle battle)
    {
        foreach (var listener in Events)
        {
            if (listener is not BattleStartedEventListenerData onBattleStartedEventListener)
                continue;

            onBattleStartedEventListener.OnBattleStarted(battle);
        }
    }

    public void DoBattleFinishedEvent(Battle battle)
    {
        foreach (var listener in Events)
        {
            if (listener is not BattleFinishedEventListenerData onBattleFinishedListener)
                continue;

            onBattleFinishedListener.OnBattleFinished(battle);
        }
    }
    
    public void DoAttackEvent(Battle battle, PawnController abilityUser, Ability ability)
    {
        foreach (var listener in Events)
        {
            if (listener is not AttackEventListenerData onAttackEventListener)
                continue;

            onAttackEventListener.OnAttack(battle, abilityUser, ability);
        }
    }

    public void DoSpecialAttackEvent(Battle battle, PawnController abilityUser, Ability ability)
    {
        foreach (var listener in Events)
        {
            if (listener is not SpecialAttackEventListenerData onSpecialAttackEventListener)
                continue;

            onSpecialAttackEventListener.OnSpecialAttack(battle, abilityUser, ability);
        }
    }

    public void DoPawnDeathEvent(Battle battle, PawnController dead, DamageDomain damage)
    {
        foreach (var listener in Events)
        {
            if (listener is not PawnDeathEventListenerData onPawnDeathListener)
                continue;

            onPawnDeathListener.OnPawnDeath(battle, dead, damage);
        }
    }
    
    public void DoHealthGainedEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in Events)
        {
            if (listener is not HealthGainedEventListenerData onHealthGainedListener)
                continue;

            onHealthGainedListener.OnHealthGained(battle, pawnController, value);
        }
    }
    
    public void DoHealthLostEvent(Battle battle, PawnController pawnController, DamageDomain damage)
    {
        foreach (var listener in Events)
        {
            if (listener is not HealthLostEventListenerData onHealthLostListener)
                continue;

            onHealthLostListener.OnHealthLost(battle, pawnController, damage);
        }
    }
    
    public void DoManaGainedEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in Events)
        {
            if (listener is not ManaGainedEventListenerData onManaGainedListener)
                continue;

            onManaGainedListener.OnManaGained(battle, pawnController, value);
        }
    }
    
    public void DoManaLostEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in Events)
        {
            if (listener is not ManaLostEventListenerData onManaLostListener)
                continue;

            onManaLostListener.OnManaLost(battle, pawnController, value);
        }
    }

    public void DoBlessingCreatedEvent()
    {
        foreach (var listener in Events)
        {
            if (listener is not BlessingCreatedEventListenerData blessingCreatedListener)
                continue;

            blessingCreatedListener.OnBlessingCreated();
        }
    }
}

[Serializable]
public abstract class BaseBattleEventListenerData : IComponentData
{
    
}

public interface IBlessingEffectData : IComponentData
{
    
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
    
    EscudoParaEquipe = 07_01,
    
    InvocaCachorro = 08_01,
}