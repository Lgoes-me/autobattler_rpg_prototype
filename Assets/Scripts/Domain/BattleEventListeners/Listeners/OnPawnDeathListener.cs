using System;
using System.Linq;

public delegate void PawnDeathDelegate(Battle battle, PawnController dead, Pawn attacker, Rarity rarity);

public class OnPawnDeathListener : BaseBattleEventListener<Func<Battle, PawnController, Pawn, bool>, PawnDeathDelegate>
{
    public void OnPawnDeath(Battle battle, PawnController dead, Pawn attacker, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle, dead, attacker)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, dead, attacker, rarity);
        }
    }
}