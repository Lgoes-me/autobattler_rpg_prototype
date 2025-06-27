using System;
using UnityEngine;

[System.Serializable]
public class DamageData
{
    [field: SerializeField] public float Multiplier { get; set; }
    [field: SerializeField] public DamageType Type { get; set; }

    public DamageDomain ToDomain(Pawn pawn)
    {
        var value = Type switch
        {
            DamageType.Physical => (int) Multiplier * pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Strength),
            DamageType.Magical => (int) Multiplier * pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Arcane),
            _ => throw new ArgumentOutOfRangeException()
        };

        value = value > 0 ? value : 0;
        
        return new DamageDomain(pawn, value, Type);
    }
}