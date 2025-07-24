using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BuffToPawnEffectData :
    IAttackEffect,
    ISpecialAttackEffect,
    IHealthGainedEffect,
    IHealthLostEffect,
    IManaGainedEffect,
    IManaLostEffect
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private int Duration { get; set; }
    [field: SerializeReference] [field: SerializeField] private List<BuffComponentData> Buffs { get; set; }
    
    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);
    public void OnManaGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);   
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        if (!pawnController.Pawn.GetComponent<ResourceComponent>().IsAlive)
            return;

        var buff = new Buff(Id, Duration);

        foreach (var buffComponentData in Buffs)
        {
            var buffComponent = buffComponentData.ToDomain(pawnController.Pawn);
            buff.Add(buffComponent);
        }

        pawnController.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
    }

}

[Serializable]
public class BuffToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private int Duration { get; set; }
    [field: SerializeReference] [field: SerializeField] private List<BuffComponentData> Buffs { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var team = Team == TeamType.Player ? battle.PlayerPawns : battle.EnemyPawns;
        
        foreach (var p in team)
        {
            var buff = new Buff(Id, Duration);

            foreach (var buffComponentData in Buffs)
            {
                var buffComponent = buffComponentData.ToDomain(p.Pawn);
                buff.Add(buffComponent);
            }

            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
}


[Serializable]
public class AddMetadataToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private string MetaDataString { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        var team = Team == TeamType.Player ? battle.PlayerPawns : battle.EnemyPawns;
        
        foreach (var p in team)
        {
            p.Pawn.GetComponent<MetaDataComponent>().AddMetaData(MetaDataString);
        }
    }
}

[Serializable]
public class AddMetadataToPawnEffectData :
    IAttackEffect,
    ISpecialAttackEffect,
    IHealthGainedEffect,
    IHealthLostEffect,
    IManaGainedEffect,
    IManaLostEffect
{
    [field: SerializeField] private string MetaDataKey { get; set; }
    
    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);
    public void OnManaGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        pawnController.Pawn.GetComponent<MetaDataComponent>().AddMetaData(MetaDataKey);
    }
}

[Serializable]
public class RemoveMetadataFromPartyEffectData : 
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
    [field: SerializeField] private string MetaDataKey { get; set; }

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
            p.Pawn.GetComponent<MetaDataComponent>().RemoveMetaData(MetaDataKey);
        }
    }
}

[Serializable]
public class RemoveMetadataFromPawnEffectData : 
    IAttackEffect,
    ISpecialAttackEffect,
    IHealthGainedEffect,
    IHealthLostEffect,
    IManaGainedEffect,
    IManaLostEffect
{
    [field: SerializeField] private string MetaDataKey { get; set; }

    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability) => DoEffect(abilityUser);
    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);
    public void OnManaGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);   
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        pawnController.Pawn.GetComponent<MetaDataComponent>().RemoveMetaData(MetaDataKey);
    }
}