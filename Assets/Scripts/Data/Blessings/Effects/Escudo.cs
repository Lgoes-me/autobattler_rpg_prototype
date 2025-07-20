using System;
using UnityEngine;

[Serializable]
public class GiveShieldToPartyEffectData : IBattleStartedEffect
{
    [field: SerializeField] private int ShieldValue { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<ResourceComponent>().GiveShield(ShieldValue);
        }
    }
}