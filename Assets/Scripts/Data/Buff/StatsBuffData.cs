using UnityEngine;

[System.Serializable]
public class StatsBuffData : BuffComponentData
{
    [field: SerializeField] private StatsData StatsVariation { get; set; }
    [field: SerializeField] private float Duration { get; set; }

    public override Buff ToDomain(string id, PawnController abilityUser)
    {
        return new StatModifierBuff(StatsVariation.ToDomain(), id, Duration);
    }
}