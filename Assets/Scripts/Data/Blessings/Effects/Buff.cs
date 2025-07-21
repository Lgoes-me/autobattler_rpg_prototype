using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffToPawnEffectData : IHealthGainedEffect, IHealthLostEffect, IManaLostEffect
{
    [field: SerializeField] public string Id { get; private set; }

    [field: SerializeReference]
    [field: SerializeField]
    private List<BuffComponentData> Buffs { get; set; }

    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);

    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(pawnController);

    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        if (pawnController.Pawn.Team != TeamType.Player)
            return;

        if (!pawnController.Pawn.GetComponent<ResourceComponent>().IsAlive)
            return;

        var buff = new Buff(Id, -1);

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
    [field: SerializeField] public string Id { get; private set; }

    [field: SerializeReference]
    [field: SerializeField]
    private List<BuffComponentData> Buffs { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.PlayerPawns)
        {
            var buff = new Buff(Id, -1);

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
public class BuffToEnemiesEffectData : IBattleStartedEffect
{
    [field: SerializeField] public string Id { get; private set; }

    [field: SerializeReference]
    [field: SerializeField]
    private List<BuffComponentData> Buffs { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.EnemyPawns)
        {
            var buff = new Buff(Id, -1);

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
    [field: SerializeField] public string MetaDataString { get; private set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<MetaDataComponent>().AddMetaData(MetaDataString);
        }
    }
}

[Serializable]
public class AddMetadataToPawmEffectData : IPawnDeathEffect
{
    [field: SerializeField] public string MetaDataKey { get; private set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        pawnController.Pawn.GetComponent<MetaDataComponent>().AddMetaData(MetaDataKey);
    }
}

[Serializable]
public class RemoveMetadataFromPartyEffectData : IBattleStartedEffect, IPawnDeathEffect
{
    [field: SerializeField] public string MetaDataKey { get; private set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);
    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<MetaDataComponent>().RemoveMetaData(MetaDataKey);
        }
    }
}

[Serializable]
public class RemoveMetadataFromPawnEffectData : IPawnDeathEffect
{
    [field: SerializeField] public string MetaDataKey { get; private set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        pawnController.Pawn.GetComponent<MetaDataComponent>().RemoveMetaData(MetaDataKey);
    }
}