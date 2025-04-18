using UnityEngine;

[CreateAssetMenu]
public class ConsumableData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    
    [field: SerializeField] [field: SerializeReference] private EffectData[] Effects { get; set; }
}
