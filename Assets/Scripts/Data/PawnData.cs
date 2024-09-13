using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] private int Health { get; set; } = 10;
    [field: SerializeField] private int Size { get; set; } = 1;
    [field: SerializeField] private int Initiative { get; set; } = 1;
    [field: SerializeField] private bool HasMana { get; set; }

    [field: SerializeField] private List<AttackData> Attacks { get; set; }
    [field: SerializeField] private List<AttackData> SpecialAttacks { get; set; }

    public PawnDomain ToDomain()
    {
        return new PawnDomain(Health, Size, Initiative, HasMana, Attacks, SpecialAttacks);
    }
}