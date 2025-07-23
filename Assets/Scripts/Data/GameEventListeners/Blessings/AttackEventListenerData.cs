using System;

[Serializable]
public class AttackEventListenerData : BaseEventListenerData<IAttackValidatorEffect, IAttackEffect>
{
    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability)
    {
        if (Validator != null && !Validator.Validate(battle, abilityUser, ability))
            return;

        Effect.OnAttack(battle, abilityUser, ability);
    }
}

public interface IAttackValidatorEffect : IEventValidatorData
{
    bool Validate(Battle battle, PawnController abilityUser, Ability ability);
}

public interface IAttackEffect : IEventEffectData
{
    void OnAttack(Battle battle, PawnController abilityUser, Ability ability);
}
