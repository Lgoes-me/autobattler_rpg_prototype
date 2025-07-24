using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class DanoParaPartyEffectData : 
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
    [field: SerializeField] private int Dano { get; set; }

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
        var damage = new DamageDomain(null, Dano, DamageType.True);
        
        foreach (var p in team)
        {
            p.Pawn.GetComponent<ResourceComponent>().ReceiveDamage(damage);
        }
    }
}

[Serializable]
public class DanoEmAreaPartindoDoPawnEffectData : 
    IPawnDeathEffect,
    IHealthLostEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int Dano { get; set; }
    [field: SerializeField] private float Range { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle, pawnController);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle, pawnController);

    private void DoEffect(Battle battle, PawnController pawnController)
    {
        var team = Team == TeamType.Player ? battle.PlayerPawns : battle.EnemyPawns;
        
        var damage = new DamageDomain(pawnController.Pawn, Dano, DamageType.True);

        var enemiesInRange = team
            .Where(p => Vector3.Distance(pawnController.transform.position, p.transform.position) < Range)
            .ToList();

        foreach (var pawn in enemiesInRange)
        {
            pawn.GetComponent<ResourceComponent>().ReceiveDamage(damage);
        }
    }
}

[Serializable]
public class DanoDeVingancaEffectData : 
    IPawnDeathEffect,
    IHealthLostEffect
{
    [field: SerializeField] private int Dano { get; set; }
    
    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController, damage);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController, damage);

    private void DoEffect(PawnController pawnController, DamageDomain damage)
    {
        if (damage.Attacker == null)
            return;

        var newDamage = new DamageDomain(pawnController.Pawn, Dano, DamageType.True);
        damage.Attacker.GetComponent<ResourceComponent>().ReceiveDamage(newDamage);
    }
}

[Serializable]
public class DanoDeVingancaEmAreaEffectData :
    IPawnDeathEffect,
    IHealthGainedEffect,
    IHealthLostEffect,
    IManaGainedEffect,
    IManaLostEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int Percentual { get; set; }
    [field: SerializeField] private float Range { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle, pawnController, damage.Value);
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(battle, pawnController, value);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle, pawnController, damage.Value);
    public void OnManaGained(Battle battle, PawnController pawnController, int value) => DoEffect(battle, pawnController, value);
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(battle, pawnController, value);

    private void DoEffect(Battle battle, PawnController pawnController, int value)
    {
        var team = Team == TeamType.Player ? battle.PlayerPawns : battle.EnemyPawns;

        var damageValue = Mathf.CeilToInt(value * Percentual / (float) 100);
        var newDamage = new DamageDomain(pawnController.Pawn, damageValue, DamageType.True);

        var enemiesInRange = team
            .Where(p => Vector3.Distance(pawnController.transform.position, p.transform.position) < Range)
            .ToList();

        foreach (var pawn in enemiesInRange)
        {
            pawn.GetComponent<ResourceComponent>().ReceiveDamage(newDamage);
        }
    }

}