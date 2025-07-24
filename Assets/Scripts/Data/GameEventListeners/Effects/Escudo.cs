using System;
using UnityEngine;

[Serializable]
public class GiveShieldToPartyEffectData : 
    IBattleStartedEffect,
    IAttackEffect,
    ISpecialAttackEffect,
    IPawnDeathEffect,
    IHealthGainedEffect,
    IHealthLostEffect,
    IManaGainedEffect,
    IManaLostEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int ShieldValue { get; set; }

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
        var team = Team == TeamType.Player ? battle.PlayerPawns : battle.EnemyPawns;
        
        foreach (var p in team)
        {
            p.Pawn.GetComponent<ResourceComponent>().GiveShield(ShieldValue);
        }
    }
}

[Serializable]
public class GiveShieldToPawnEffectData : 
    IAttackEffect,
    ISpecialAttackEffect,
    IHealthGainedEffect,
    IHealthLostEffect,
    IManaGainedEffect,
    IManaLostEffect
{
    [field: SerializeField] private int ShieldValue { get; set; }

    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);
    public void OnManaGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);   
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    
    private void DoEffect(PawnController abilityUser)
    {
        abilityUser.Pawn.GetComponent<ResourceComponent>().GiveShield(ShieldValue);
    }
}