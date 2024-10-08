using UnityEngine;

[System.Serializable]
public class DamageEffectData : BaseEffectData
{
    [field: SerializeField] private Damage Damage { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new DamageEffect(abilityUser, Damage);
    }
}