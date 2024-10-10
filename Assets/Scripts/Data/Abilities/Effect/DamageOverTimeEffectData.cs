using UnityEngine;

[System.Serializable]
public class DamageOverTimeEffectData : BaseEffectData
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private DamageData Damage { get; set; }
    [field: SerializeField] private float Duration { get; set; }
    [field: SerializeField] private float TickRate { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var damage = Damage.ToDomain(abilityUser.Pawn);
        var damageOverTimeBuff = new DamageOverTimeBuff(damage, TickRate, Id, Duration);
        return new DamageOverTimeEffect(abilityUser, damageOverTimeBuff);
    }
}