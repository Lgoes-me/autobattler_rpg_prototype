using UnityEngine;

[System.Serializable]
public class AttackBuffEffectData : BaseEffectData
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private DamageType Type { get; set; }
    [field: SerializeField] private int Variation { get; set; }
    [field: SerializeField]  private float Duration { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var damageOverTimeBuff = new AttackValueBuff(Type, Variation, Id, Duration);
        return new SimpleBuffEffect(abilityUser, damageOverTimeBuff);
    }
}