﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] public string Id { get;  set; }
    [field: SerializeField] private int Health { get; set; }
    [field: SerializeField] private int Mana { get; set; }
    [field: SerializeField] private CharacterController Character { get; set; }
    [field: SerializeField] private WeaponController Weapon { get; set; }
    [field: SerializeField] private StatsData Stats { get; set; }
    [field: SerializeField] private List<AbilityData> Abilities { get; set; }
    [field: SerializeField] private List<AbilityData> SpecialAbilities { get; set; }
    [field: SerializeField] private List<ArchetypeIdentifier> Archetypes { get; set; }

    public Pawn ToDomain()
    {
        var stats = Stats.ToDomain();
        return new Pawn(
            Id, 
            Health, 
            Mana, 
            Character, 
            Weapon, 
            stats,
            Abilities, 
            SpecialAbilities, 
            Archetypes);
    }
    
    public PawnFacade ToFacade()
    {
        return new PawnFacade(
            Id,
            Character, 
            Weapon);
    }

    private void OnValidate()
    {
        if (Id != string.Empty)
            return;

        Id = name;
    }
}