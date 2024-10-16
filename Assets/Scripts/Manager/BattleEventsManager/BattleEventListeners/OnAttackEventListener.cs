using System;
using System.Linq;

public class OnAttackEventListener : BaseBattleEventListener<Func<PawnDomain, Ability, bool>, AttackDelegate>
{
    public void OnAttack(PawnDomain abilityUser, Ability ability)
    {
        if (Validators.Any(validator => !validator(abilityUser, ability)))
            return;

        foreach (AttackDelegate modifier in Modifiers)
        {
            modifier(abilityUser, ability);
        }
    }
}

public delegate void AttackDelegate(PawnDomain abilityUser, Ability ability);