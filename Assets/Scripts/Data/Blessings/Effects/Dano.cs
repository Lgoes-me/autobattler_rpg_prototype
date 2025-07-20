using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ReduzDanoPercentualmenteToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private int PercentReduction { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var stat = new StatsData()
        {
            new StatData(Stat.DamageModifier, -PercentReduction),
        };

        var buff = new Buff(BlessingIdentifier.ReduzODanoRecebido.ToString(), -1)
        {
            new StatModifierBuff(stat.ToDomain())
        };

        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
}

[Serializable]
public class DanoParaEnemyPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private int Dano { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var damage = new DamageDomain(null, Dano, DamageType.True);

        foreach (var p in battle.EnemyPawns)
        {
            p.Pawn.GetComponent<ResourceComponent>().ReceiveDamage(damage);
        }
    }
}

[Serializable]
public class DanoEmAreaPartindoDoAliadoEffectData : IPawnDeathEffect
{
    [field: SerializeField] private int Dano { get; set; }
    [field: SerializeField] private float Range { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(battle, pawnController);

    private void DoEffect(Battle battle, PawnController pawnController)
    {
        if (pawnController.Pawn.Team != TeamType.Player)
            return;

        var damage = new DamageDomain(pawnController.Pawn, Dano, DamageType.True);

        var enemiesInRange = battle.EnemyPawns
            .Where(p => Vector3.Distance(pawnController.transform.position, p.transform.position) < Range)
            .ToList();

        foreach (var pawn in enemiesInRange)
        {
            pawn.GetComponent<ResourceComponent>().ReceiveDamage(damage);
        }
    }
}

[Serializable]
public class DanoEmAreaPartindoDoInimigoEffectData : IPawnDeathEffect
{
    [field: SerializeField] private int Dano { get; set; }
    [field: SerializeField] private float Range { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(battle, pawnController);

    private void DoEffect(Battle battle, PawnController pawnController)
    {
        if (pawnController.Pawn.Team != TeamType.Enemies)
            return;

        var damage = new DamageDomain(null, Dano, DamageType.True);

        var enemiesInRange = battle.EnemyPawns
            .Where(p => Vector3.Distance(pawnController.transform.position, p.transform.position) < Range)
            .ToList();

        foreach (var pawn in enemiesInRange)
        {
            pawn.GetComponent<ResourceComponent>().ReceiveDamage(damage);
        }
    }
}

[Serializable]
public class DanoDeVingancaEffectData : IPawnDeathEffect
{
    [field: SerializeField] private int Dano { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(pawnController, damage);

    private void DoEffect(PawnController pawnController, DamageDomain damage)
    {
        if (pawnController.Pawn.Team != TeamType.Player || damage.Attacker == null)
            return;

        var newDamage = new DamageDomain(pawnController.Pawn, Dano, DamageType.True);
        damage.Attacker.GetComponent<ResourceComponent>().ReceiveDamage(newDamage);
    }
}

[Serializable]
public class DanoDeVingancaEmAreaEffectData : IPawnDeathEffect, IHealthLostEffect, IHealthGainedEffect
{
    [field: SerializeField] private int Percentual { get; set; }
    [field: SerializeField] private float Range { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(battle, pawnController, damage.Value);

    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(battle, pawnController, damage.Value);
    
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) =>
        DoEffect(battle, pawnController, value);

    private void DoEffect(Battle battle, PawnController pawnController, int value)
    {
        if (pawnController.Pawn.Team != TeamType.Player)
            return;

        var damageValue = Mathf.CeilToInt(value * Percentual / (float) 100);
        var newDamage = new DamageDomain(pawnController.Pawn, damageValue, DamageType.True);

        var enemiesInRange = battle.EnemyPawns
            .Where(p => Vector3.Distance(pawnController.transform.position, p.transform.position) < Range)
            .ToList();

        foreach (var pawn in enemiesInRange)
        {
            pawn.GetComponent<ResourceComponent>().ReceiveDamage(newDamage);
        }
    }
}