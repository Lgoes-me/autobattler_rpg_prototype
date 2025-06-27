using System;
using System.Linq;

public delegate void PawnDamagedDelegate(Battle battle, PawnController dead, DamageDomain damage, Rarity rarity);

public class OnPawnDeathListener : BaseBattleEventListener<Func<Battle, PawnController, DamageDomain, bool>, PawnDamagedDelegate>
{
    public void OnPawnDeath(Battle battle, PawnController dead, DamageDomain damage, Rarity rarity)
    {
        if (Validators.Any(validator => !validator(battle, dead, damage)))
            return;

        foreach (var modifier in Modifiers)
        {
            modifier(battle, dead, damage, rarity);
        }
    }
}