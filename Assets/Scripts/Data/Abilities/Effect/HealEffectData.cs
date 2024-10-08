using UnityEngine;

[System.Serializable]
public class HealEffectData : BaseEffectData
{
    [field: SerializeField] private float HealMultiplier { get; set; }
    [field: SerializeField] private bool CanRevive { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new HealEffect(abilityUser, HealMultiplier, CanRevive);
    }
}