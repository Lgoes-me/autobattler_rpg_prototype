using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SummonCreatureEffectData : IBattleEffect
{
    [field: SerializeField] private EnemyData Summon { get; set; }

    public void OnBattleStateChanged(Battle battle) => DoEffect(battle);

    private void DoEffect(Battle battle)
    {
        battle.PlayerPawns.First().SummonPawn(Summon);
    }
}