﻿using System;
using System.Linq;

public delegate void BlessingDelegate(Rarity rarity);

public class BlessingGainedListener : BaseBattleEventListener<Func<bool>, BlessingDelegate>
{
    public void OnBlessingGained(Rarity rarity)
    {
        if (Validators.Any(validator => !validator()))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(rarity);
        }
    }
}

public class BlessingCreatedListener : BaseBattleEventListener<Func<bool>, BlessingDelegate>
{
    public void OnBlessingCreated(Rarity rarity)
    {
        if (Validators.Any(validator => !validator()))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(rarity);
        }
    }
}