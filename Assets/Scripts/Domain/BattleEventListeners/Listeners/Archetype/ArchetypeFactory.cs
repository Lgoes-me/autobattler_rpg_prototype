using System;

public class ArchetypeFactory
{
    public Archetype CreateArchetype(ArchetypeIdentifier id, int currentAmount)
    {
        return id switch
        {
            ArchetypeIdentifier.Teste => new Archetype(id, currentAmount, new[] {2, 4, 6})
            {
            },

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}