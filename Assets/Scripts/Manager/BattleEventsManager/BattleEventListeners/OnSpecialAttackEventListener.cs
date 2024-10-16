using System;
using System.Linq;

public class OnSpecialAttackEventListener : BaseBattleEventListener<Func<PawnDomain, Ability, bool>, AttackDelegate>
{
    public void OnSpecialAttack(PawnDomain abilityUser, Ability ability)
    {
        if (Validators.Any(validator => !validator(abilityUser, ability)))
            return;

        foreach (AttackDelegate modifier in Modifiers)
        {
            modifier(abilityUser, ability);
        }
    }
}
