using System;
using System.Linq;

public class OnAttackEventListener : BaseBattleEventListener<Func<Battle, PawnController, Ability, bool>, AttackDelegate>
{
    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability)
    {
        if (Validators.Any(validator => !validator(battle, abilityUser, ability)))
            return;

        foreach (AttackDelegate modifier in Modifiers)
        {
            modifier(battle, abilityUser, ability);
        }
    }
}

public delegate void AttackDelegate(Battle battle, PawnController abilityUser, Ability ability);