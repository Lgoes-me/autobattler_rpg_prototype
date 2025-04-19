using UnityEngine;

[System.Serializable]
public class StatsBuffData : BuffComponentData
{
    [field: SerializeField] private StatsData StatsVariation { get; set; }

    public override BuffComponent ToDomain(PawnController abilityUser)
    {
        return new StatModifierBuff(StatsVariation.ToDomain());
    }
}