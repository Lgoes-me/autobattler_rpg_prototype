using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SummonCreatureEffectData : 
    IBattleStartedEffect,
    IAttackEffect,
    ISpecialAttackEffect,
    IPawnDeathEffect,
    IHealthGainedEffect,
    IHealthLostEffect,
    IManaGainedEffect,
    IManaLostEffect
{
    [field: SerializeField] private EnemyData Summon { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);
    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(battle);
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(battle);
    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle);
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(battle);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle);
    public void OnManaGained(Battle battle, PawnController pawnController, int value) => DoEffect(battle);    
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(battle);
    
    
    private void DoEffect(Battle battle)
    {
        battle.PlayerPawns.First().SummonPawn(Summon);
    }
}