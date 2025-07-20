using System;
using UnityEngine;

[Serializable]
public class BonusDeStatEffectData : IHealthGainedEffect , IHealthLostEffect, IManaLostEffect
{
    [field: SerializeField] private BuffData BuffData { get; set; }

    public void OnHealthGained(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(pawnController);
    public void OnManaLost(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    
    private void DoEffect(PawnController pawnController)
    {
        if (pawnController.Pawn.Team != TeamType.Player)
            return;

        var buff = BuffData.ToDomain(pawnController.Pawn, -1);
        
        if (!pawnController.Pawn.GetComponent<ResourceComponent>().IsAlive)
            return;

        pawnController.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
    }
}