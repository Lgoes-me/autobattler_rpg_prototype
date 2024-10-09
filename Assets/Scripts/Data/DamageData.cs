using UnityEngine;

[System.Serializable]
public class DamageData
{
    [field: SerializeField] public float Multiplier { get; set; }
    [field: SerializeField] public DamageType Type { get; set; }

    public DamageDomain ToDomain(PawnDomain pawn)
    {
        return new DamageDomain(pawn, Multiplier, Type);
    }
}