using System;
using System.Linq;

public class OnSpecialAttackEventListener : BaseBattleEventListener<Func<Battle, PawnController, Ability, bool>, AttackDelegate>
{
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability)
    {
        if (Validators.Any(validator => !validator(battle, abilityUser, ability)))
            return;

        foreach (AttackDelegate modifier in Modifiers)
        {
            modifier(battle, abilityUser, ability);
        }
    }
}
