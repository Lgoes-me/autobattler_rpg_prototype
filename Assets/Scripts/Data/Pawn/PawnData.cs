using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] private int Initiative { get; set; }
    [field: SerializeField] private int VisionRange { get; set; }
    [field: SerializeField] private int AttackRange { get; set; }
    [field: SerializeField] private float RangedAttackError { get; set; }
    [field: SerializeField] private FocusType EnemyFocusPreference { get; set; }
    [field: SerializeField] private FocusType AllyFocusPreference { get; set; }
    [field: SerializeField] private CharacterController Character { get; set; }
    [field: SerializeField] private StatsData BaseStats { get; set; }
    [field: SerializeField] private LevelUpStatsData LevelUpStats { get; set; }
    [field: SerializeField] public List<AbilityData> Abilities { get; private set; }
    [field: SerializeField] private List<ArchetypeIdentifier> Archetypes { get; set; }
    [field: SerializeField] private List<CharacterInfo> CharacterInfos { get; set; }
    [field: SerializeField] private WeaponType WeaponType { get; set; }
    [field: SerializeField] public WeaponData Weapon { get; private set; }

    public Pawn ToDomain(TeamType team, int level)
    {
        return new Pawn(
            level,
            Id,
            Initiative,
            VisionRange,
            AttackRange,
            RangedAttackError,
            Character,
            BaseStats.ToDomain(),
            LevelUpStats.ToDomain(),
            EnemyFocusPreference,
            AllyFocusPreference,
            Abilities.ToList(),
            Archetypes,
            team,
            CharacterInfos,
            Weapon,
            WeaponType);
    }

    public BasePawn ToBaseDomain()
    {
        return new BasePawn(
            Id,
            Character,
            CharacterInfos,
            Abilities.ToList(),
            Weapon,
            WeaponType);
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
    [field: SerializeField] public string Identifier { get; set; }
    [field: SerializeField] public Sprite Portrait { get; set; }
    [field: SerializeField] public Sfx Audio { get; set; }
}