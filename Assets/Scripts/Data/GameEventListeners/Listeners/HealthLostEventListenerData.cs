using System;

[Serializable]
public class HealthLostEventListenerData : BaseEventListenerData<IHealthLostValidator, IHealthLostEffect>
{
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, damage))
            return;
        
        Effect.OnHealthLost(battle, pawnController, damage);
    }
}

public interface IHealthLostValidator : IEventValidatorData
{
    bool Validate(Battle battle, PawnController pawnController, DamageDomain damage);
}

public interface IHealthLostEffect : IEventEffectData
{
    void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage);
}