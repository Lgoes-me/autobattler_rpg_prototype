using System;
using System.Linq;

public class OnAttackEventListener : BaseBattleEventListener<Func<PawnController, Ability, bool>, AttackDelegate>
{
    public void OnAttack(PawnController abilityUser, Ability ability)
    {
        if (Validators.Any(validator => !validator(abilityUser, ability)))
            return;

        foreach (AttackDelegate modifier in Modifiers)
        {
            modifier(abilityUser, ability);
        }
    }
}

public delegate void AttackDelegate(PawnController abilityUser, Ability ability);