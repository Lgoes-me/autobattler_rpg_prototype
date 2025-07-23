using System;

[Serializable]
public class BattleFinishedEventListenerData : BaseEventListenerData<IBattleFinishedValidatorEffect, IBattleFinishedEffect>
{
    public void OnBattleFinished(Battle battle)
    {
        if (Validator != null && !Validator.Validate(battle))
            return;

        Effect.OnBattleFinished(battle);
    }
}

public interface IBattleFinishedValidatorEffect : IEventValidatorData
{
    bool Validate(Battle battle);
}

public interface IBattleFinishedEffect : IEventEffectData
{
    void OnBattleFinished(Battle battle);
}