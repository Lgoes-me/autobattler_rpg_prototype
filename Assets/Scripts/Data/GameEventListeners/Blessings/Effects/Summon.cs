using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SummonCreatureEffectData : IBattleStartedEffect
{
    [field: SerializeField] private EnemyData Summon { get; set; }

    public void OnBattleStarted(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        battle.PlayerPawns.First().SummonPawn(Summon);
    }
}