using System;

[Serializable]
public class SpecialAttackEventListenerData : BaseEventListenerData<IAttackValidator, IAttackEffect>
{
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability)
    {
        if (Validator != null && !Validator.Validate(battle, abilityUser, ability))
            return;
        
        Effect.OnAttack(battle, abilityUser, ability);
    }
}