using System;
using System.Linq;

public class OnAttackEventListener : BaseBattleEventListener<Func<Battle, PawnController, Ability, bool>, AttackDelegate>
{
    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability)
    {
        if (Validators.Any(validator => !validator(battle, abilityUser, ability)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, abilityUser, ability);
        }
    }
}

public class OnSpecialAttackEventListener : BaseBattleEventListener<Func<Battle, PawnController, Ability, bool>, AttackDelegate>
{
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability)
    {
        if (Validators.Any(validator => !validator(battle, abilityUser, ability)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, abilityUser, ability);
        }
    }
}

public delegate void AttackDelegate(Battle battle, PawnController abilityUser, Ability ability);