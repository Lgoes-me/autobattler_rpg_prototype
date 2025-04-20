using UnityEngine;

[System.Serializable]
public class DamageOverTimeBuffData : BuffComponentData
{
    [field: SerializeField] private DamageData Damage { get; set; }
    [field: SerializeField] private float TickRate { get; set; }

    public override BuffComponent ToDomain(Pawn pawn)
    {
        var damage = Damage.ToDomain(pawn);
        return new DamageOverTimeBuff(damage, TickRate);
    }
}