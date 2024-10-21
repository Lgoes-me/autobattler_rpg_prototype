using System;
using System.Linq;

public class OnSpecialAttackEventListener : BaseBattleEventListener<Func<PawnController, Ability, bool>, AttackDelegate>
{
    public void OnSpecialAttack(PawnController abilityUser, Ability ability)
    {
        if (Validators.Any(validator => !validator(abilityUser, ability)))
            return;

        foreach (AttackDelegate modifier in Modifiers)
        {
            modifier(abilityUser, ability);
        }
    }
}
