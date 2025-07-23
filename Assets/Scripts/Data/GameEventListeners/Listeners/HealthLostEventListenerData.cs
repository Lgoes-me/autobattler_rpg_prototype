using System;

[Serializable]
public class HealthLostEventListenerData : BaseEventListenerData<IDamageReceivedValidator, IDamageReveivedEffect>
{
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, damage))
            return;
        
        Effect.OnDamageReceived(battle, pawnController, damage);
    }
}

public interface IDamageReceivedValidator : IEventValidatorData
{
    bool Validate(Battle battle, PawnController pawnController, DamageDomain damage);
}

public interface IDamageReveivedEffect : IEventEffectData
{
    void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage);
}