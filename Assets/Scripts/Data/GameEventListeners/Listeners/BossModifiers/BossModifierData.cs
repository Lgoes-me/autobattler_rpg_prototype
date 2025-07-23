using UnityEngine;

[CreateAssetMenu]
public class BossModifierData : BaseGameEventListenerData
{
    [field: SerializeField] private Rarity Rarity { get; set; }
}