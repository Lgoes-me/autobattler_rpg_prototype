using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    
    [field: SerializeField] public CharacterController Character { get; private set; }
    [field: SerializeField] private int Health { get; set; } = 10;
    [field: SerializeField] private int Size { get; set; } = 1;
    [field: SerializeField] private int Initiative { get; set; } = 1;

    [field: SerializeField] private List<AttackData> Attacks { get; set; }
    [field: SerializeField] private List<AttackData> SpecialAttacks { get; set; }

    public PawnDomain ToDomain()
    {
        return new PawnDomain(Health, Size, Initiative, Attacks, SpecialAttacks);
    }

    private void OnValidate()
    {
        if (Id != string.Empty)
            return;

        Id = name;
    }
}