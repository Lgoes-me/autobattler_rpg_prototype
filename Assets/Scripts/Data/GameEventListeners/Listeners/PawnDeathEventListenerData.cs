using System;

[Serializable]
public class PawnDeathEventListenerData : BaseEventListenerData<IDamageReceivedValidator, IDamageReveivedEffect>
{
    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, damage))
            return;

        Effect.OnDamageReceived(battle, pawnController, damage);
    }
}