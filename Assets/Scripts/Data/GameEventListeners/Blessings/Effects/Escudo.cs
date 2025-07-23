using System;
using UnityEngine;

[Serializable]
public class GiveShieldToPartyEffectData : IBattleStartedEffect, ISpecialAttackEffect
{
    [field: SerializeField] private int ShieldValue { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(battle);
    
    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<ResourceComponent>().GiveShield(ShieldValue);
        }
    }
}