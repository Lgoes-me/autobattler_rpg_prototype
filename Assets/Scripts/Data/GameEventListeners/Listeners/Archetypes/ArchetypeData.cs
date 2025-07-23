using System;
using UnityEngine;

[CreateAssetMenu]
public class ArchetypeData : BaseGameEventListenerData
{
    [field: SerializeField] public ArchetypeIdentifier Identifier { get; set; }
    [field: SerializeField] public ArchetypeStep[] Archetypes { get; set; }
    
    public ArchetypeStep CurrentArchetype { get; private set; }
    public ArchetypeStep NextArchetype { get; private set; }
    
    public override Rarity GetRarity() => CurrentArchetype.Rarity;
    protected override BaseEvent[] GetEvents() => CurrentArchetype.Events;
    
    public void Setup(int pawns)
    {
        CurrentArchetype = null;
        NextArchetype = null;
        
        foreach (var archetype in Archetypes)
        {
            if(archetype.Pawns > pawns)
            {
                NextArchetype = archetype;
                break;
            }
            
            CurrentArchetype = archetype;
        }
    }
}

[Serializable]
public class ArchetypeStep
{
    [field: SerializeField] public int Pawns { get; set; }
    
    [field: SerializeField] public Rarity Rarity { get; set; }
    [field: SerializeField] [field: SerializeReference] public BaseEvent[] Events { get; set; }
}


public enum ArchetypeIdentifier
{
    Unknown,
    Cavaleiros,
    Magos,
    Herois,
    Weakener,
    Hunters,
}