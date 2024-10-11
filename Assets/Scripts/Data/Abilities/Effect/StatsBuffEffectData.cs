using UnityEngine;

[System.Serializable]
public class StatsBuffEffectData : BaseEffectData
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private StatsData StatsVariation { get; set; }
    [field: SerializeField] private float Duration { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var buff = new StatModifierBuff(StatsVariation.ToDomain(), Id, Duration);
        return new SimpleBuffEffect(abilityUser, buff);
    }
}