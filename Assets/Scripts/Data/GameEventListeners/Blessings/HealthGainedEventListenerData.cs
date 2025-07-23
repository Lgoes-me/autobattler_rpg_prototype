using System;

[Serializable]
public class HealthGainedEventListenerData : BaseEventListenerData<IHealthGainedValidatorEffect, IHealthGainedEffect>
{
    public void OnHealthGained(Battle battle, PawnController pawnController, int value)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, value))
            return;

        Effect.OnHealthGained(battle, pawnController, value);
    }
}

public interface IHealthGainedValidatorEffect : IEventValidatorData
{
    bool Validate(Battle battle, PawnController pawnController, int value);
}

public interface IHealthGainedEffect : IEventEffectData
{
    void OnHealthGained(Battle battle, PawnController pawnController, int value);
}
