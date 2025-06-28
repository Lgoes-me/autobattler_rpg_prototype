using System;

public class BossModifierFactory
{
    public BossModifier CreateArchetype(BossModifierIdentifier id)
    {
        return id switch
        {
            BossModifierIdentifier.Unknown => new BossModifier(id)
            {
            },

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}