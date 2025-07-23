using System;
using UnityEngine;

[CreateAssetMenu]
public class ArchetypeData : BaseGameEventListenerData
{
    protected override BaseEventListenerData[] Events => CurrentArchetypeGroup.Events;
    
    [field: SerializeField] private ArchetypeGroup[] ArchetypeGroups { get; set; }
    private ArchetypeGroup CurrentArchetypeGroup { get; set; }
    
    public int CurrentAmount { get; }
    public int[] AmountSteps { get; }
}

[Serializable]
public class ArchetypeGroup
{
    [field: SerializeField] [field: SerializeReference] public BaseEventListenerData[] Events { get; set; }
}