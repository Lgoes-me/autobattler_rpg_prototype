using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class PawnDataComponent : IComponentData
{
    public abstract PawnComponent ToDomain();
}

public class WeaponListComponentData : PawnDataComponent
{
    [field: SerializeField] private List<WeaponData> Weapons { get; set; }
    
    public override PawnComponent ToDomain()
    {
        return new WeaponComponent(Weapons.Select(w => w.ToDomain()).ToList());
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
        return new AbilitiesComponent(Abilities.ToList());
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

    public override PawnComponent ToDomain()
    {
        return new StatsComponent(BaseStats.ToDomain());
    }
}

public class LevelUpStatsComponentData : PawnDataComponent
{
    [field: SerializeField] private LevelUpStatsData LevelUpStats { get; set; }

    public override PawnComponent ToDomain()
    {
        return new LevelUpStatsComponent(LevelUpStats.ToDomain());
    }
}

public class PawnBuffsComponentData : PawnDataComponent
{
    public override PawnComponent ToDomain()
    {
        return new PawnBuffsComponent();
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

public class MountComponentData : PawnDataComponent
{
    [field: SerializeField] public CharacterController Mount { get; set; }

    public override PawnComponent ToDomain()
    {
        return new MountComponent(Mount);
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

public class ConsumableComponentData : PawnDataComponent
{
    public override PawnComponent ToDomain()
    {
        return new ConsumableComponent();
    }
}