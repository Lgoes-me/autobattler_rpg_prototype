using System;
using UnityEngine;

[Serializable]
public class RegeneracaoDeManaToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private int RegenValue { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var buff = new Buff(BlessingIdentifier.RegeneracaoDeMana.ToString(), -1)
        {
            new ManaRegenBuff(RegenValue, 2)
        };

        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
}

[Serializable]
public class ManaToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private int ManaValue { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<ResourceComponent>().GiveMana(ManaValue);
        }
    }
}

[Serializable]
public class AumentaManaRecebidaPercentualmenteToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private int PercentIncrease { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var stat = new StatsData()
        {
            new StatData(Stat.ManaGainModifier, PercentIncrease),
        };

        var buff = new Buff(BlessingIdentifier.DobraAQuantidadeDeManaRecebida.ToString(), -1)
        {
            new StatModifierBuff(stat.ToDomain())
        };
        
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
}


[Serializable]
public class PercentualDoDanoViraManaEffectData : IHealthLostEffect, IHealthGainedEffect
{
    [field: SerializeField] private int Percentual { get; set; }

    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(battle, pawnController, damage.Value);
    
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) =>
        DoEffect(battle, pawnController, value);

    private void DoEffect(Battle battle, PawnController pawnController, int value)
    {
        if (pawnController.Pawn.Team != TeamType.Player)
            return;

        var manaValue = Mathf.CeilToInt(value * Percentual / (float) 100);

        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<ResourceComponent>().GiveMana(manaValue);
        }
    }
}