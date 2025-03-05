using System;
using System.Linq;

public class OnBattleStartedListener : BaseBattleEventListener<Func<Battle, bool>, BattleStartedDelegate>
{
    public void OnBattleStarted(Battle battle)
    {
        if (Validators.Any(validator => !validator(battle)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle);
        }
    }
}

public delegate void BattleStartedDelegate(Battle battle);