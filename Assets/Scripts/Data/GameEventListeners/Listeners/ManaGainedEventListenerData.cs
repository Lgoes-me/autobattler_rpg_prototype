using System;

[Serializable]
public class ManaGainedEventListenerData : BaseEventListenerData<IManaGainedValidator, IManaGainedEffect>
{
    public void OnManaGained(Battle battle, PawnController pawnController, int value)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, value))
            return;

        Effect.OnManaGained(battle, pawnController, value);
    }
}

public interface IManaGainedValidator : IEventValidatorData
{
    bool Validate(Battle battle, PawnController pawnController, int value);
}

public interface IManaGainedEffect : IEventEffectData
{
    void OnManaGained(Battle battle, PawnController pawnController, int value);
}
