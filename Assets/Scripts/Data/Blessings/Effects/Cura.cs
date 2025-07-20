﻿using System;
using UnityEngine;

[Serializable]
public class HealToPartyEffectData : IBattleStartedEffect, IBattleFinishedEffect
{
    [field: SerializeField] private int HealValue { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);
    public void OnBattleFinished(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(HealValue, false);
        }
    }
}

[Serializable]
public class RegenToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private int RegenValue { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var buff = new Buff(BlessingIdentifier.CuraAoLongoDoTempo.ToString(), -1)
        {
            new RegenBuff(RegenValue, 2)
        };

        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
}

[Serializable]
public class AumentaCuraPercentualmenteToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private int HealPowerValue { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var stats = new StatsData()
        {
            new StatData(Stat.HealPower, HealPowerValue),
        };

        var buff = new Buff(BlessingIdentifier.AumentaCuraPercentualmente.ToString(), -1)
        {
            new StatModifierBuff(stats.ToDomain())
        };

        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
}

[Serializable]
public class HealPlayerPawnEffectData : IPawnDeathEffect, IAttackEffect, ISpecialAttackEffect, IManaLostEffect
{
    [field: SerializeField] private int HealValue { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(battle, pawnController);

    public void OnAttack(Battle battle, PawnController pawnController, Ability ability) =>
        DoEffect(battle, pawnController);

    public void OnSpecialAttack(Battle battle, PawnController pawnController, Ability ability) =>
        DoEffect(battle, pawnController);

    public void OnManaLost(Battle battle, PawnController pawnController, int value) =>
        DoEffect(battle, pawnController);
    
    private void DoEffect(Battle battle, PawnController pawnController)
    {
        if (pawnController.Pawn.Team != TeamType.Player)
            return;

        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(HealValue, false);
        }
    }
}

[Serializable]
public class HealPercentualPlayerPawnEffectData : IHealthLostEffect
{
    [field: SerializeField] private int Percentual { get; set; }

    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(pawnController, damage);
    
    private void DoEffect(PawnController pawnController, DamageDomain damage)
    {
        if (pawnController.Pawn.Team != TeamType.Enemies || damage.Attacker == null)
            return;

        var healValue = Mathf.CeilToInt(damage.Value * Percentual / (float) 100);
        damage.Attacker.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false);
    }

}