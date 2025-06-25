using System;
using System.Linq;

public delegate void BattleDelegate(Battle battle, Rarity rarity);

public class OnBattleStartedListener : BaseBattleEventListener<Func<Battle, bool>, BattleDelegate>
{
    public void OnBattleStarted(Battle battle, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, rarity);
        }
    }
}

public class OnBattleFinishedListener : BaseBattleEventListener<Func<Battle, bool>, BattleDelegate>
{
    public void OnBattleFinished(Battle battle, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, rarity);
        }
    }
}

