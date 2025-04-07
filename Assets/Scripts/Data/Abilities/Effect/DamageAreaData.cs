using UnityEngine;

[System.Serializable]
public class DamageAreaData : EffectData
{
    [field: SerializeField] private DamageData Damage { get; set; }
    [field: SerializeField] private int Range { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var damage = Damage.ToDomain(abilityUser.Pawn);
        return new DamageAreaEffect(abilityUser, damage, Range);
    }
}