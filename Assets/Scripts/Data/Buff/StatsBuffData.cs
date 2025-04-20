using UnityEngine;

[System.Serializable]
public class StatsBuffData : BuffComponentData
{
    [field: SerializeField] private StatsData StatsVariation { get; set; }

    public override BuffComponent ToDomain(Pawn pawn)
    {
        return new StatModifierBuff(StatsVariation.ToDomain());
    }
}