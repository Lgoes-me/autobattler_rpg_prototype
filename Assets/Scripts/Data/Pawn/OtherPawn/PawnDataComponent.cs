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
    [field: SerializeField] public WeaponData Weapon { get; private set; }
    
    public override PawnComponent ToDomain()
    {
        throw new NotImplementedException();
    }
}

public class CharacterInfoComponentData : PawnDataComponent
{
    [field: SerializeField] private List<CharacterInfo> CharacterInfos { get; set; }
    
    public override PawnComponent ToDomain()
    {
        throw new NotImplementedException();
    }
}

public class ArchetypesComponentData : PawnDataComponent
{
    [field: SerializeField] private List<ArchetypeIdentifier> Archetypes { get; set; }
    
    public override PawnComponent ToDomain()
    {
        throw new NotImplementedException();
    }
}

public class AbilitiesComponentData : PawnDataComponent
{
    [field: SerializeField] public List<AbilityData> Abilities { get; private set; }
    
    public override PawnComponent ToDomain()
    {
        throw new NotImplementedException();
    }
}

public class EnemyComponentData : PawnDataComponent
{
    [field: SerializeField] private int Initiative { get; set; }
    [field: SerializeField] private int VisionRange { get; set; }
    [field: SerializeField] private int AttackRange { get; set; }
    
    public override PawnComponent ToDomain()
    {
        throw new NotImplementedException();
    }
}

public class StatsComponentData : PawnDataComponent
{
    [field: SerializeField] private StatsData BaseStats { get; set; }
    [field: SerializeField] private LevelUpStatsData LevelUpStats { get; set; }
    
    public override PawnComponent ToDomain()
    {
        throw new NotImplementedException();
    }
    
}

public class CharacterComponentData : PawnDataComponent
{
    [field: SerializeField] private CharacterController Character { get; set; }
    
    public override PawnComponent ToDomain()
    {
        throw new NotImplementedException();
    }
}

public class FocusComponentData : PawnDataComponent
{
    [field: SerializeField] private float RangedAttackError { get; set; }
    [field: SerializeField] private FocusType EnemyFocusPreference { get; set; }
    [field: SerializeField] private FocusType AllyFocusPreference { get; set; }
    
    public override PawnComponent ToDomain()
    {
        throw new NotImplementedException();
    }
}