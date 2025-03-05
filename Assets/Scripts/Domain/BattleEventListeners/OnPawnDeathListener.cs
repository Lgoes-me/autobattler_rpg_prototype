using System;
using System.Linq;

public class OnPawnDeathListener : BaseBattleEventListener<Func<Battle, PawnController, bool>, PawnDeathDelegate>
{
    public void OnPawnDeath(Battle battle, PawnController pawnController, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle, pawnController)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, pawnController, rarity);
        }
    }
}

public delegate void PawnDeathDelegate(Battle battle, PawnController pawnController, Rarity rarity);