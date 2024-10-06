﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    
    [field: SerializeField] public CharacterController Character { get; private set; }
    [field: SerializeField] public WeaponController Weapon { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] private int Mana { get; set; }
    
    [field: SerializeField] private int Strength { get; set; }
    
    [field: SerializeField] private List<AbilityData> Abilities { get; set; }
    [field: SerializeField] private List<AbilityData> SpecialAbilities { get; set; }

    public PawnDomain ToDomain()
    {
        return new PawnDomain(Id, Health, Mana, Strength, Abilities, SpecialAbilities);
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