using System;
using UnityEngine;

[Serializable]
public class ManaToPartyEffectData : IBattleEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int ManaValue { get; set; }

    public void OnBattleStateChanged(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var team = Team == TeamType.Player ? battle.PlayerPawns : battle.EnemyPawns;
        
        foreach (var p in team)
        {
            p.Pawn.GetComponent<ResourceComponent>().GiveMana(ManaValue);
        }
    }
}

[Serializable]
public class PercentualDoDanoViraManaEffectData : IDamageReveivedEffect, IResourceChangedEffect
{
    [field: SerializeField] private int Percentual { get; set; }

    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController, damage.Value);
    public void OnResourceChanged(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController, value);

    private void DoEffect(PawnController pawnController, int value)
    {
        var manaValue = Mathf.CeilToInt(value * Percentual / (float) 100);
        pawnController.Pawn.GetComponent<ResourceComponent>().GiveMana(manaValue);
    }
}