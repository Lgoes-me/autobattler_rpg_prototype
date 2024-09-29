using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    
    [field: SerializeField] public CharacterController Character { get; private set; }
    [field: SerializeField] public int Health { get; private set; } = 10;
    [field: SerializeField] private int Size { get; set; } = 1;
    [field: SerializeField] private int Initiative { get; set; } = 1;

    [field: SerializeField] private List<AttackData> Attacks { get; set; }
    [field: SerializeField] private List<AttackData> SpecialAttacks { get; set; }

    public PawnDomain ToDomain()
    {
        return new PawnDomain(Id, Health, Size, Initiative, Attacks, SpecialAttacks);
    }
    
    public PawnInfo ResetPawnInfo()
    {
        return new PawnInfo(Id, Health);
    }

    private void OnValidate()
    {
        if (Id != string.Empty)
            return;

        Id = name;
    }
}