using UnityEngine;

[System.Serializable]
public class DamageEffectData : EffectData
{
    [field: SerializeField] private DamageData Damage { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var damage = Damage.ToDomain(abilityUser.Pawn);
        return new DamageEffect(abilityUser, damage);
    }
}