using System;
using UnityEngine;

[Serializable]
public class BattleStartedEventListenerData : BaseEvent
{
    [field: SerializeField] [field: SerializeReference] protected IBattleStartedEffect Effect { get; set; }
    
    public void OnBattleStarted(Battle battle)
    {
        Effect.OnBattleStarted(battle);
    }
}

public interface IBattleStartedValidator : IEventValidatorData
{
    bool Validate(Battle battle);
}

public interface IBattleStartedEffect : IEventEffectData
{
    void OnBattleStarted(Battle battle);
}