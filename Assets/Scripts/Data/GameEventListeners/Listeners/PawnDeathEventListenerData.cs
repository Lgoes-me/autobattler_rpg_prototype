using System;

[Serializable]
public class PawnDeathEventListenerData : BaseEventListenerData<IPawnDeathValidator, IPawnDeathEffect>
{
    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, damage))
            return;

        Effect.OnPawnDeath(battle, pawnController, damage);
    }
}
public interface IPawnDeathValidator : IEventValidatorData
{
    bool Validate(Battle battle, PawnController pawnController, DamageDomain damage);
}

public interface IPawnDeathEffect : IEventEffectData
{
    void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage);
}