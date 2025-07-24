using System;
using UnityEngine;

[Serializable]
public class BattleFinishedEventListenerData : BaseEvent
{
    [field: SerializeField] [field: SerializeReference] protected IBattleFinishedEffect Effect { get; set; }
    
    public void OnBattleFinished(Battle battle)
    {
        Effect.OnBattleFinished(battle);
    }
}

public interface IBattleFinishedValidator : IEventValidatorData
{
    bool Validate(Battle battle);
}

public interface IBattleFinishedEffect : IEventEffectData
{
    void OnBattleFinished(Battle battle);
}