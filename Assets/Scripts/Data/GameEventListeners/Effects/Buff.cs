using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BuffToPawnEffectData : IResourceChangedEffect, IDamageReveivedEffect
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private int Duration { get; set; }
    [field: SerializeReference] [field: SerializeField] private List<BuffComponentData> Buffs { get; set; }

    public void OnResourceChanged(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);

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
public class BuffToPartyEffectData : IBattleEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private int Duration { get; set; }
    [field: SerializeReference] [field: SerializeField] private List<BuffComponentData> Buffs { get; set; }

    public void OnBattleStateChanged(Battle battle) => DoEffect(battle);

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
public class AddMetadataToPartyEffectData : IBattleEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private string MetaDataString { get; set; }

    public void OnBattleStateChanged(Battle battle) => DoEffect(battle);

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
public class AddMetadataToPawnEffectData : IDamageReveivedEffect
{
    [field: SerializeField] private string MetaDataKey { get; set; }

    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        pawnController.Pawn.GetComponent<MetaDataComponent>().AddMetaData(MetaDataKey);
    }
}

[Serializable]
public class RemoveMetadataFromPartyEffectData : IBattleEffect, IDamageReveivedEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private string MetaDataKey { get; set; }

    public void OnBattleStateChanged(Battle battle) => DoEffect(battle);
    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle);

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
public class RemoveMetadataFromPawnEffectData : IDamageReveivedEffect
{
    [field: SerializeField] private string MetaDataKey { get; set; }

    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        pawnController.Pawn.GetComponent<MetaDataComponent>().RemoveMetaData(MetaDataKey);
    }
}