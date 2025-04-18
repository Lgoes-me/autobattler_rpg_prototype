using UnityEngine;

[CreateAssetMenu]
public class ConsumableData : ScriptableObject
{
    [field: SerializeField] [field: SerializeReference] private EffectData[] Effects { get; set; }
}
