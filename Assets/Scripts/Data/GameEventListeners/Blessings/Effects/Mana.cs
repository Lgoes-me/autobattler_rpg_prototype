using System;
using UnityEngine;

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