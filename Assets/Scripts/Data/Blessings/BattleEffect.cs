using System;
using UnityEngine;

[Serializable]
public class BattleStartedEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IBattleStartedEffect Effect { get; set; }

    public void OnBattleStarted(Battle battle) => Effect.OnBattleStarted(battle);
}

public interface IBattleStartedEffect : IBlessingEffectData
{
    void OnBattleStarted(Battle battle);
}

[Serializable]
public class BattleFinishedEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IBattleFinishedEffect Effect { get; set; }

    public void OnBattleFinished(Battle battle) => Effect.OnBattleFinished(battle);
}

public interface IBattleFinishedEffect : IBlessingEffectData
{
    void OnBattleFinished(Battle battle);
}