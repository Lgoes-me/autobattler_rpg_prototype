using System;

[Serializable]
public class BattleStartedEventListenerData : BaseEventListenerData<IBattleStartedValidatorEffect, IBattleStartedEffect>
{
    public void OnBattleStarted(Battle battle)
    {
        if (Validator != null && !Validator.Validate(battle))
            return;

        Effect.OnBattleStarted(battle);
    }
}

public interface IBattleStartedValidatorEffect : IEventValidatorData
{
    bool Validate(Battle battle);
}

public interface IBattleStartedEffect : IEventEffectData
{
    void OnBattleStarted(Battle battle);
}