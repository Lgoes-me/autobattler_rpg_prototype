using System;
using System.Linq;

public class OnPawnDeathListener : BaseBattleEventListener<Func<Battle, PawnController, bool>, PawnDeathDelegate>
{
    public void OnPawnDeath(Battle battle, PawnController pawnController)
    {
        if (Validators.Any(validator => !validator(battle, pawnController)))
            return;

        foreach (PawnDeathDelegate modifier in Modifiers)
        {
            modifier(battle, pawnController);
        }
    }
}

public delegate void PawnDeathDelegate(Battle battle, PawnController pawnController);