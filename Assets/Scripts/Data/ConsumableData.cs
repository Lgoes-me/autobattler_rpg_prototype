using UnityEngine;

[CreateAssetMenu]
public class ConsumableData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public bool UsableOutsideCombat { get; private set; }
    [field: SerializeField] [field: SerializeReference] public EffectData Effect { get; private set; }
}
