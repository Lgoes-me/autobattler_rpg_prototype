using UnityEngine;

[CreateAssetMenu]
public class BlessingData : BaseGameEventListenerData
{
    [field: SerializeField] private Rarity Rarity { get; set; }
    [field: SerializeField] [field: SerializeReference] private BaseEvent[] Events { get; set; }
    
    public override Rarity GetRarity() => Rarity;
    protected override BaseEvent[] GetEvents() => Events;
}