using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class DanoParaPartyEffectData : IBattleEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int Dano { get; set; }

    public void OnBattleStateChanged(Battle battle) => DoEffect(battle);

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
public class DanoEmAreaPartindoDoPawnEffectData : IDamageReveivedEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int Dano { get; set; }
    [field: SerializeField] private float Range { get; set; }

    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle, pawnController);

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
public class DanoDeVingancaEffectData : IDamageReveivedEffect
{
    [field: SerializeField] private int Dano { get; set; }

    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController, damage);

    private void DoEffect(PawnController pawnController, DamageDomain damage)
    {
        if (damage.Attacker == null)
            return;

        var newDamage = new DamageDomain(pawnController.Pawn, Dano, DamageType.True);
        damage.Attacker.GetComponent<ResourceComponent>().ReceiveDamage(newDamage);
    }
}

[Serializable]
public class DanoDeVingancaEmAreaEffectData : IDamageReveivedEffect, IResourceChangedEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int Percentual { get; set; }
    [field: SerializeField] private float Range { get; set; }

    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle, pawnController, damage.Value);
    public void OnResourceChanged(Battle battle, PawnController pawnController, int value) => DoEffect(battle, pawnController, value);

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