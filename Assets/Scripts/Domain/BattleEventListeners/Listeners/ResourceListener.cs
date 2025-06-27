using System;
using System.Linq;

public delegate void ResourceDelegate(Battle battle, PawnController pawnController, int value, Rarity rarity);


public class OnHealthLostListener : BaseBattleEventListener<Func<Battle, PawnController, DamageDomain, bool>, PawnDamagedDelegate>
{
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle, pawnController, damage)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, pawnController, damage, rarity);
        }
    }
}

public class OnHealthGainedListener : BaseBattleEventListener<Func<Battle, PawnController, bool>, ResourceDelegate>
{
    public void OnHealthGained(Battle battle, PawnController pawnController, int value, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle, pawnController)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, pawnController, value, rarity);
        }
    }
}

public class OnManaLostListener : BaseBattleEventListener<Func<Battle, PawnController, bool>, ResourceDelegate>
{
    public void OnManaLost(Battle battle, PawnController pawnController, int value, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle, pawnController)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, pawnController, value, rarity);
        }
    }
}

public class OnManaGainedListener : BaseBattleEventListener<Func<Battle, PawnController, bool>, ResourceDelegate>
{
    public void OnManaGained(Battle battle, PawnController pawnController, int value, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle, pawnController)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, pawnController, value, rarity);
        }
    }
}