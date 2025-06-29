using System;

public class BossModifierFactory
{
    public BossModifier CreateBossModifier(BossModifierIdentifier id, Rarity rarity)
    {
        return id switch
        {
            BossModifierIdentifier.Unknown => new BossModifier(id, rarity)
            {
            },

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}