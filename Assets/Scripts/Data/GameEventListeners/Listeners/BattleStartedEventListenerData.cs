using System;
using UnityEngine;

[Serializable]
public class BattleStartedEventListenerData : BaseEvent
{
    [field: SerializeField] [field: SerializeReference] protected IBattleEffect Effect { get; set; }
    
    public void OnBattleStarted(Battle battle)
    {
        Effect.OnBattleStateChanged(battle);
    }
}

public interface IBattleValidator : IEventValidatorData
{
    bool Validate(Battle battle);
}

public interface IBattleEffect : IEventEffectData
{
    void OnBattleStateChanged(Battle battle);
}