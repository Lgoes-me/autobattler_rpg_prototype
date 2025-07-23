using System;

[Serializable]
public class ManaLostEventListenerData : BaseEventListenerData<IManaLostValidatorEffect, IManaLostEffect>
{
    public void OnManaLost(Battle battle, PawnController pawnController, int value)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, value))
            return;

        Effect.OnManaLost(battle, pawnController, value);
    }
}
public interface IManaLostValidatorEffect : IEventValidatorData
{
    bool Validate(Battle battle, PawnController pawnController, int value);
}

public interface IManaLostEffect : IEventEffectData
{
    void OnManaLost(Battle battle, PawnController pawnController, int value);
}