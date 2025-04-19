using UnityEngine;

[System.Serializable]
public class DamageOverTimeBuffData : BuffComponentData
{
    [field: SerializeField] private DamageData Damage { get; set; }
    [field: SerializeField] private float Duration { get; set; }
    [field: SerializeField] private float TickRate { get; set; }

    public override Buff ToDomain(string id, PawnController abilityUser)
    {
        var damage = Damage.ToDomain(abilityUser.Pawn);
        return new DamageOverTimeBuff(damage, TickRate, id, Duration);
    }
}