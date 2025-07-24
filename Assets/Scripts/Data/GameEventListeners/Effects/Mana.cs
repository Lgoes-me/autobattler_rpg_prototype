using System;
using UnityEngine;

[Serializable]
public class ManaToPartyEffectData :
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
    [field: SerializeField] private int ManaValue { get; set; }

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
            p.Pawn.GetComponent<ResourceComponent>().GiveMana(ManaValue);
        }
    }
}

[Serializable]
public class PercentualDoDanoViraManaEffectData :
    IHealthGainedEffect,
    IHealthLostEffect,
    IManaGainedEffect,
    IManaLostEffect
{
    [field: SerializeField] private int Percentual { get; set; }
    
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController, value);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController, damage.Value);
    public void OnManaGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController, value);
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController, value);
    
    private void DoEffect(PawnController pawnController, int value)
    {
        var manaValue = Mathf.CeilToInt(value * Percentual / (float) 100);
        pawnController.Pawn.GetComponent<ResourceComponent>().GiveMana(manaValue);
    }
}