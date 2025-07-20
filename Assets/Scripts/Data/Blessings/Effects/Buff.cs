using System;
using UnityEngine;

[Serializable]
public class BuffToPawnEffectData : IHealthGainedEffect , IHealthLostEffect, IManaLostEffect
{
    [field: SerializeField] private BuffData BuffData { get; set; }

    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    
    private void DoEffect(PawnController pawnController)
    {
        if (pawnController.Pawn.Team != TeamType.Player)
            return;
        
        if (!pawnController.Pawn.GetComponent<ResourceComponent>().IsAlive)
            return;

        var buff = BuffData.ToDomain(pawnController.Pawn, -1);

        pawnController.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
    }
}

[Serializable]
public class BuffToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private BuffData BuffData { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.PlayerPawns)
        {
            var buff = BuffData.ToDomain(p.Pawn, -1);
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
}