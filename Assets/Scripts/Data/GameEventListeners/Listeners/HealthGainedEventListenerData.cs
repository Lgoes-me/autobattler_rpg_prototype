using System;

[Serializable]
public class HealthGainedEventListenerData : BaseEventListenerData<IHealthGainedValidator, IHealthGainedEffect>
{
    public void OnHealthGained(Battle battle, PawnController pawnController, int value)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, value))
            return;

        Effect.OnHealthGained(battle, pawnController, value);
    }
}

public interface IHealthGainedValidator : IEventValidatorData
{
    bool Validate(Battle battle, PawnController pawnController, int value);
}

public interface IHealthGainedEffect : IEventEffectData
{
    void OnHealthGained(Battle battle, PawnController pawnController, int value);
}
