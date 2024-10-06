using UnityEngine;

[System.Serializable]
public class HealEffectData : BaseEffectData
{
    [field: SerializeField] private int HealValue { get; set; }
    [field: SerializeField] private bool CanRevive { get; set; }

    public override AbilityEffect ToDomain()
    {
        return new HealEffect(HealValue, CanRevive);
    }
}