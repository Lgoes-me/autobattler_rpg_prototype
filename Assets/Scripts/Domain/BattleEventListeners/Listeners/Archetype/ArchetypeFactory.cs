﻿using System;

public class ArchetypeFactory
{
    public Archetype CreateArchetype(ArchetypeIdentifier id, int quantidade)
    {
        return id switch
        {
            ArchetypeIdentifier.Teste => new Archetype(id, quantidade)
            {
                
            },

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}