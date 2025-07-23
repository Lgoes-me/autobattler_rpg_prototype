using System;

[Serializable]
public class HealthGainedEventListenerData : BaseEventListenerData<IResourceChangedValidator, IResourceChangedEffect>
{
    public void OnHealthGained(Battle battle, PawnController pawnController, int value)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, value))
            return;

        Effect.OnResourceChanged(battle, pawnController, value);
    }
}

public interface IResourceChangedValidator : IEventValidatorData
{
    bool Validate(Battle battle, PawnController pawnController, int value);
}

public interface IResourceChangedEffect : IEventEffectData
{
    void OnResourceChanged(Battle battle, PawnController pawnController, int value);
}
