using System;

[Serializable]
public class SpecialAttackEventListenerData : BaseEventListenerData<ISpecialAttackValidator, ISpecialAttackEffect>
{
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability)
    {
        if (Validator != null && !Validator.Validate(battle, abilityUser, ability))
            return;
        
        Effect.OnSpecialAttack(battle, abilityUser, ability);
    }
}
public interface ISpecialAttackValidator : IEventValidatorData
{
    bool Validate(Battle battle, PawnController abilityUser, Ability ability);
}

public interface ISpecialAttackEffect : IEventEffectData
{
    void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability);
}
