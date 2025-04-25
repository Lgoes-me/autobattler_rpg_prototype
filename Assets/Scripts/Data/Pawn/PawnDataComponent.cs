using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class PawnDataComponent : IComponentData
{
    public abstract PawnComponent ToDomain();
}

public class WeaponComponentData : PawnDataComponent
{
    [field: SerializeField] private WeaponType WeaponType { get; set; }
    [field: SerializeField] private WeaponData Weapon { get; set; }

    public override PawnComponent ToDomain()
    {
        return new WeaponComponent(Weapon, WeaponType);
    }
}

public class CharacterInfoComponentData : PawnDataComponent
{
    [field: SerializeField] private List<CharacterInfo> CharacterInfos { get; set; }

    public override PawnComponent ToDomain()
    {
        return new CharacterInfoComponent(CharacterInfos);
    }
}

public class ArchetypesComponentData : PawnDataComponent
{
    [field: SerializeField] private List<ArchetypeIdentifier> Archetypes { get; set; }

    public override PawnComponent ToDomain()
    {
        return new ArchetypesComponent(Archetypes);
    }
}

public class AbilitiesComponentData : PawnDataComponent
{
    [field: SerializeField] private List<AbilityData> Abilities { get; set; }

    public override PawnComponent ToDomain()
    {
        return new AbilitiesComponent(Abilities);
    }
}

public class EnemyComponentData : PawnDataComponent
{
    [field: SerializeField] private int VisionRange { get; set; }
    [field: SerializeField] private int AttackRange { get; set; }

    public override PawnComponent ToDomain()
    {
        return new EnemyComponent(VisionRange, AttackRange);
    }
}

public class StatsComponentData : PawnDataComponent
{
    [field: SerializeField] private StatsData BaseStats { get; set; }
    [field: SerializeField] private LevelUpStatsData LevelUpStats { get; set; }

    public override PawnComponent ToDomain()
    {
        return new StatsComponent(BaseStats.ToDomain(), LevelUpStats.ToDomain());
    }
}

public class CharacterComponentData : PawnDataComponent
{
    [field: SerializeField] private CharacterController Character { get; set; }

    public override PawnComponent ToDomain()
    {
        return new CharacterComponent(Character);
    }
}

public class FocusComponentData : PawnDataComponent
{
    [field: SerializeField] private float RangedAttackError { get; set; }
    [field: SerializeField] private FocusType EnemyFocusPreference { get; set; }
    [field: SerializeField] private FocusType AllyFocusPreference { get; set; }

    public override PawnComponent ToDomain()
    {
        return new FocusComponent(RangedAttackError, EnemyFocusPreference, AllyFocusPreference);
    }
}