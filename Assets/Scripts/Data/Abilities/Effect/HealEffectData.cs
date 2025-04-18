using UnityEngine;

[System.Serializable]
public class HealEffectData : EffectData
{
    [field: SerializeField] private float HealMultiplier { get; set; }
    [field: SerializeField] private bool CanRevive { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new HealEffect(abilityUser, HealMultiplier, CanRevive);
    }
}

[System.Serializable]
public class StaticHealEffectData : EffectData
{
    [field: SerializeField] private int HealValue { get; set; }
    [field: SerializeField] private bool CanRevive { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new StaticHealEffect(abilityUser, HealValue, CanRevive);
    }
}