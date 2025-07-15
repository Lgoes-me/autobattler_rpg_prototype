﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PawnComponent
{
    protected Pawn Pawn { get; private set; }

    public virtual void Init(Pawn pawn)
    {
        Pawn = pawn;
    }

    public virtual void SetPawnInfo(PawnInfo pawnInfo)
    {
    }
}

public class WeaponComponent : PawnComponent
{
    public Weapon Weapon { get; private set; }
    public WeaponType WeaponType { get; private set; }
    public WeaponController WeaponPrefab { get; private set; }

    private List<Weapon> Weapons { get; }

    public WeaponComponent(List<Weapon> weapons)
    {
        Weapons = weapons;

        Weapon = Weapons.First();
        WeaponType = Weapon.Type;
        WeaponPrefab = Weapon.WeaponPrefab;
    }

    public void ApplyWeaponType(WeaponType weaponType)
    {
        if (Weapon != null)
            return;

        Weapon = Weapons.FirstOrDefault(w => w.Type == weaponType) ?? Weapons.First();

        WeaponType = Weapon.Type;
        WeaponPrefab = Weapon.WeaponPrefab;
    }

    public override void SetPawnInfo(PawnInfo pawnInfo)
    {
        if (string.IsNullOrWhiteSpace(pawnInfo.Weapon))
            return;

        Weapon = Application.Instance.GetManager<ContentManager>().GetWeaponFromId(pawnInfo.Weapon).ToDomain();
        WeaponType = Weapon.Type;
        WeaponPrefab = Weapon.WeaponPrefab;
    }
}

public class CharacterInfoComponent : PawnComponent
{
    private List<CharacterInfo> CharacterInfos { get; set; }

    public CharacterInfoComponent(List<CharacterInfo> characterInfos)
    {
        CharacterInfos = characterInfos;
    }

    public CharacterInfo GetCharacterInfo(string identifier)
    {
        return CharacterInfos.FirstOrDefault(i => i.Identifier == identifier) ??
               CharacterInfos.First(i => i.Identifier == "default");
    }
}

public class ArchetypesComponent : PawnComponent
{
    public List<ArchetypeIdentifier> Archetypes { get; }

    public ArchetypesComponent(List<ArchetypeIdentifier> archetype)
    {
        Archetypes = archetype;
    }
}


public class AbilitiesComponent : PawnComponent
{
    public List<AbilityData> Abilities { get; private set; }

    public AbilitiesComponent(List<AbilityData> abilities)
    {
        Abilities = abilities;
    }

    public Ability GetCurrentAttackIntent(
        PawnController abilityUser,
        Battle battle)
    {
        var abilities = new List<AbilityData>();

        abilities.AddRange(Abilities.Where(a => a.ResourceData.HasResource(abilityUser)).ToList());

        var highestValue = abilities
            .ToDictionary(a => a, a => a.GetPriority(abilityUser, battle))
            .OrderBy(p => p.Value).Last().Value;

        var selected = abilities
            .ToDictionary(a => a, a => a.GetPriority(abilityUser, battle))
            .OrderBy(p => p.Value)
            .Where(k => k.Value == highestValue)
            .Select(p => p.Key)
            .OrderBy(_ => Guid.NewGuid())
            .First();

        return selected.ToDomain(abilityUser);
    }

    public override void SetPawnInfo(PawnInfo pawnInfo)
    {
        foreach (var ability in pawnInfo.Abilities)
        {
            if (Abilities.Any(a => a.Id == ability))
                continue;

            Abilities.Add(Application.Instance.GetManager<ContentManager>().GetAbilityFromId(ability));
        }
    }
}

public class EnemyComponent : PawnComponent
{
    public int VisionRange { get; }
    public int AttackRange { get; }

    public EnemyComponent(int visionRange, int attackRange)
    {
        VisionRange = visionRange;
        AttackRange = attackRange;
    }
}

public class StatsComponent : PawnComponent
{
    public int Level { get; private set; }
    private Stats Stats { get; }
    private LevelUpStats LevelUpStats { get; }

    public StatsComponent(Stats stats, LevelUpStats levelUpStats)
    {
        Stats = stats;
        LevelUpStats = levelUpStats;
        Level = 0;
    }

    public void ApplyLevel(int level)
    {
        Level = level;
        LevelUpStats.EvaluateLevel(level);
    }

    public void LevelUp(int currentExperience)
    {
        Level = LevelUpStats.EvaluateExperience(currentExperience);
        LevelUpStats.EvaluateLevel(Level);
    }
    
    public Stats GetStats()
    {
        var stats = Stats + LevelUpStats.CurrentStats;

        if (Pawn.TryGetComponent<WeaponComponent>(out var weaponComponent))
        {
            stats += weaponComponent.Weapon.Stats;
        }

        if (Pawn.TryGetComponent<PawnBuffsComponent>(out var pawnBuffsComponent))
        {
            foreach (var (_, buff) in pawnBuffsComponent.Buffs)
            {
                foreach (var buffComponent in buff)
                {
                    if (buffComponent is not StatModifierBuff statModifierBuff)
                        continue;

                    stats = statModifierBuff.ProcessStats(stats);
                }
            }
        }

        return stats;
    }

    public override void SetPawnInfo(PawnInfo pawnInfo)
    {
        ApplyLevel(pawnInfo.Level);
    }
}

public class PawnBuffsComponent : PawnComponent
{
    public List<string> PermanentBuffs { get; private set; }
    public Dictionary<string, Buff> Buffs { get; private set; }
    public event Pawn.PawnDomainChanged BuffsChanged;

    public PawnBuffsComponent()
    {
        PermanentBuffs = new List<string>();
        Buffs = new Dictionary<string, Buff>();
    }

    public void StartBattle()
    {
        Buffs = new Dictionary<string, Buff>();

        foreach (var buff in PermanentBuffs)
        {
            var buffInstance = Application.Instance.GetManager<ContentManager>().GetBuffFromId(buff).ToDomain(Pawn, -1);
            buffInstance.Init(Pawn);
            AddBuff(buffInstance);
        }
    }

    public void EndBattle()
    {
        RemoveAllBuffs();
    }

    public bool AddBuff(Buff newBuff)
    {
        if (Buffs.TryGetValue(newBuff.Id, out var buff))
        {
            buff.TryReapplyBuff();
            return false;
        }

        Buffs.Add(newBuff.Id, newBuff);

        BuffsChanged?.Invoke();
        return true;
    }

    public void TickAllBuffs()
    {
        for (var index = Buffs.Count - 1; index >= 0; index--)
        {
            var buff = Buffs.ElementAt(index);

            if (!buff.Value.Tick())
            {
                RemoveBuff(buff.Value);
            }
        }
    }

    private void RemoveAllBuffs()
    {
        for (var index = Buffs.Count - 1; index >= 0; index--)
        {
            var buff = Buffs.ElementAt(index);
            RemoveBuff(buff.Value);
        }
    }

    private void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff.Id);
        BuffsChanged?.Invoke();
    }

    public override void SetPawnInfo(PawnInfo pawnInfo)
    {
        PermanentBuffs = pawnInfo.Buffs;
    }
}

public class CharacterComponent : PawnComponent
{
    public CharacterController Character { get; }

    public CharacterComponent(CharacterController character)
    {
        Character = character;
    }
}

public class MountComponent : PawnComponent
{
    public CharacterController Mount { get; }

    public MountComponent(CharacterController mount)
    {
        Mount = mount;
    }
}

public class FocusComponent : PawnComponent
{
    public float RangedAttackError { get; }
    public FocusType EnemyFocusPreference { get; }
    public FocusType AllyFocusPreference { get; }

    public FocusComponent(float rangedAttackError, FocusType enemyFocusPreference, FocusType allyFocusPreference)
    {
        RangedAttackError = rangedAttackError;
        EnemyFocusPreference = enemyFocusPreference;
        AllyFocusPreference = allyFocusPreference;
    }
}

public class ConsumableComponent : PawnComponent
{
    public List<ConsumableData> Consumables { get; }
    public event Pawn.PawnDomainChanged ConsumablesUpdated;

    public ConsumableComponent()
    {
        Consumables = new List<ConsumableData>();
    }

    public override void SetPawnInfo(PawnInfo pawnInfo)
    {
        foreach (var id in pawnInfo.Consumables)
        {
            var consumable = Application.Instance.GetManager<ContentManager>().GetConsumableFromId(id);
            Consumables.Add(consumable);
        }

        ConsumablesUpdated?.Invoke();
    }

    public void RemoveConsumable(ConsumableData consumable)
    {
        Consumables.Remove(consumable);

        ConsumablesUpdated?.Invoke();
    }
}

public class ResourceComponent : PawnComponent
{
    public int Health => StatsComponent.GetStats().GetStat(Stat.Health) - MissingHealth;
    public int MissingHealth { get; private set; }
    
    public int Mana { get; private set; }
    public int Experience { get; private set; }

    public bool HasMana => StatsComponent.GetStats().GetStat(Stat.Mana) > 0;
    public bool IsAlive => Health > 0;

    public event Pawn.PawnDomainChanged<DamageDomain> LostLife;
    public event Pawn.PawnDomainChanged<int> GainedLife;
    
    public event Pawn.PawnDomainChanged<int> LostMana;
    public event Pawn.PawnDomainChanged<int> GainedMana;
    
    public event Pawn.PawnDomainChanged<int> GainedExperience;
    public event Pawn.PawnDomainChanged<int> GainedLevel;
    
    private StatsComponent StatsComponent { get; set; }

    public ResourceComponent()
    {
        MissingHealth = 0;
        Mana = 0;
        Experience = 0;
    }

    public override void Init(Pawn pawn)
    {
        base.Init(pawn);
        StatsComponent = Pawn.GetComponent<StatsComponent>();
    }

    public void StartBattle()
    {
        Mana = 0;
    }

    public void ReceiveDamage(DamageDomain damage)
    {
        var reducedDamage = StatsComponent.GetStats().GetReducedDamage(damage);
        
        MissingHealth = Mathf.Clamp(MissingHealth + reducedDamage, 0, StatsComponent.GetStats().GetStat(Stat.Health));
        LostLife?.Invoke(damage);
    }

    public void ReceiveHeal(int healValue, bool canRevive)
    {
        if (!canRevive && !IsAlive)
            return;

        var stats = StatsComponent.GetStats();
        
        healValue = (int) (healValue + healValue * StatsComponent.GetStats().GetStat(Stat.HealPower) / (float) 100);

        MissingHealth = Mathf.Clamp(MissingHealth - healValue, 0, stats.GetStat(Stat.Health));
        GainedLife?.Invoke(healValue);
    }

    public void EndOfBattleHeal()
    {
        var healValue = 15;
        ReceiveHeal(healValue, true);
    }

    public void GainMana()
    {
        GiveMana(10);
    }
    
    public void GiveMana(int value)
    {
        value = (int) (value + value * StatsComponent.GetStats().GetStat(Stat.ManaGainModifier) / (float) 100);
        
        Mana = Mathf.Clamp(Mana + value, 0, StatsComponent.GetStats().GetStat(Stat.Mana));
        GainedMana?.Invoke(value);
    }

    public void SpentMana(int manaCost)
    {
        Mana = Mathf.Clamp(Mana - manaCost, 0, StatsComponent.GetStats().GetStat(Stat.Mana));
        LostMana?.Invoke(manaCost);
    }
    
    public void GiveXp(int combatEncounterExperience)
    {
        Experience += combatEncounterExperience;
        GainedExperience?.Invoke(combatEncounterExperience);
        
        var currentLevelExperience = StatsComponent.GetStats().GetStat(Stat.ExperienceToLevelUp);
        
        if (currentLevelExperience > 0 && Experience >= currentLevelExperience)
        {
            StatsComponent.LevelUp(Experience);
            GainedLevel?.Invoke(StatsComponent.Level);
        }
    }

    public override void SetPawnInfo(PawnInfo pawnInfo)
    {
        MissingHealth = pawnInfo.MissingHealth;
        Experience = pawnInfo.Experience;
        Mana = 0;
    }
}

public class MetaDataComponent : PawnComponent
{
    private List<string> Data { get; }

    public MetaDataComponent()
    {
        Data = new List<string>();
    }

    public void EndBattle()
    {
        Data.Clear();
    }

    public void AddMetaData(string data)
    {
        Data.Add(data);
    }

    public bool CheckMetaData(string data)
    {
        return Data.Contains(data);
    }
}