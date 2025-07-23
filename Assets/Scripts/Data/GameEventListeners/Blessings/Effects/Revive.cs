using System;
using UnityEngine;

[Serializable]
public class RevivePrimeiroAliadoAMorrerEmCombateEffectData : IPawnDeathEffect
{
    [field: SerializeField] private string MetaDataKey { get; set; }
    [field: SerializeField] private int PercentHealing { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(battle, pawnController);

    private void DoEffect(Battle battle, PawnController pawnController)
    {
        if (pawnController.Pawn.Team != TeamType.Player ||
            !pawnController.Pawn.GetComponent<MetaDataComponent>().CheckMetaData(MetaDataKey))
            return;

        var health = pawnController.Pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Health);
        var healValue = Mathf.CeilToInt(health * PercentHealing / (float) 100);

        pawnController.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, true);

        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<MetaDataComponent>().RemoveMetaData(MetaDataKey);
        }
    }
}

[Serializable]
public class ReviveTodosAliadosAMorrerEmCombateEffectData : IPawnDeathEffect
{
    [field: SerializeField] private string MetaDataKey { get; set; }
    [field: SerializeField] private int PercentHealing { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage) =>
        DoEffect(pawnController);

    private void DoEffect(PawnController pawnController)
    {
        if (pawnController.Pawn.Team != TeamType.Player ||
            !pawnController.Pawn.GetComponent<MetaDataComponent>().CheckMetaData(MetaDataKey))
            return;

        var health = pawnController.Pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Health);
        var healValue = Mathf.CeilToInt(health * PercentHealing / (float) 100);

        pawnController.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, true);
        pawnController.Pawn.GetComponent<MetaDataComponent>().RemoveMetaData(MetaDataKey);
    }
}