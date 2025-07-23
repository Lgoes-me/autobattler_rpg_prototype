using System;
using UnityEngine;

[Serializable]
public class GiveShieldToPartyEffectData : IBattleEffect, IAttackEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int ShieldValue { get; set; }

    public void OnBattleStateChanged(Battle battle) => DoEffect(battle);
    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(battle);
    
    private void DoEffect(Battle battle)
    {
        var team = Team == TeamType.Player ? battle.PlayerPawns : battle.EnemyPawns;
        
        foreach (var p in team)
        {
            p.Pawn.GetComponent<ResourceComponent>().GiveShield(ShieldValue);
        }
    }
}

[Serializable]
public class GiveShieldToPawnEffectData : IAttackEffect
{
    [field: SerializeField] private int ShieldValue { get; set; }

    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    
    private void DoEffect(PawnController abilityUser)
    {
        abilityUser.Pawn.GetComponent<ResourceComponent>().GiveShield(ShieldValue);
    }
}