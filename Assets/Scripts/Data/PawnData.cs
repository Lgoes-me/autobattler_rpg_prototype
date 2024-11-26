using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] private int Health { get; set; }
    [field: SerializeField] private int Mana { get; set; }
    [field: SerializeField] private CharacterController Character { get; set; }
    [field: SerializeField] private WeaponController Weapon { get; set; }
    [field: SerializeField] private StatsData Stats { get; set; }
    [field: SerializeField] private List<AbilityData> Abilities { get; set; }
    [field: SerializeField] private List<AbilityData> SpecialAbilities { get; set; }
    [field: SerializeField] private List<ArchetypeIdentifier> Archetypes { get; set; }
    [field: SerializeField] private List<CharacterInfo> CharacterInfos { get; set; }

    public Pawn ToDomain(TeamType team)
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
            Archetypes,
            team,
            CharacterInfos);
    }
    
    public BasePawn ToBaseDomain()
    {
        return new BasePawn(
            Id,
            Character, 
            Weapon,
            CharacterInfos);
    }

    private void OnValidate()
    {
        if (Id != string.Empty)
            return;

        Id = name;
    }
}

[Serializable]
public class CharacterInfo
{
    [field:SerializeField] public string Identifier { get; set; }
    [field:SerializeField] public Sprite Portrait { get; set; }
    [field:SerializeField] public Sfx Audio { get; set; }
}