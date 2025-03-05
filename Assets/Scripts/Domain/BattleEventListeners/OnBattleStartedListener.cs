using System;
using System.Linq;

public class OnBattleStartedListener : BaseBattleEventListener<Func<Battle, bool>, BattleStartedDelegate>
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

public delegate void BattleStartedDelegate(Battle battle, Rarity rarity);