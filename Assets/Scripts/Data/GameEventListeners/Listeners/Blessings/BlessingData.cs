using UnityEngine;

[CreateAssetMenu]
public class BlessingData : BaseGameEventListenerData
{
    [field: SerializeField] public Rarity Rarity { get; private set; }
}