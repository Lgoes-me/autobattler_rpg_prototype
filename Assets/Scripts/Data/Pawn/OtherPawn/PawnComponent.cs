﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PawnComponent
{
}

public class WeaponComponent : PawnComponent
{
    public WeaponData Weapon { get; protected set; }
    public WeaponType WeaponType { get; private set; }

    public WeaponComponent(WeaponData weapon, WeaponType weaponType)
    {
        Weapon = weapon;
        WeaponType = weaponType;
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

        abilities.AddRange(Abilities.Where(a => a.ResourceData.GetCost() <= abilityUser.Pawn.Mana).ToList());

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
}

public class EnemyComponent : PawnComponent
{
    public int Initiative { get; }
    public int VisionRange { get; }
    public int AttackRange { get; }

    public EnemyComponent(int initiative, int visionRange, int attackRange)
    {
        Initiative = initiative;
        VisionRange = visionRange;
        AttackRange = attackRange;
    }
}

public class StatsComponent : PawnComponent
{
    public int Health { get; internal set; }
    public int Mana { get; private set; }

    private Stats Stats { get; }
    private LevelUpStats LevelUpStats { get; }

    public List<string> PermanentBuffs { get; private set; }
    public Dictionary<string, Buff> Buffs { get; private set; }

    public bool HasMana => Stats.Mana > 0;
    public bool IsAlive => Health > 0;

    public delegate void PawnDomainChanged();
    public event PawnDomainChanged StatsChanged;
    
    public delegate void PawnDomainBattleStateChanged();
    public event PawnDomainBattleStateChanged BattleStarted;
    public event PawnDomainBattleStateChanged BattleFinished;
    
    public StatsComponent(Stats stats, LevelUpStats levelUpStats)
    {
        Stats = stats;
        LevelUpStats = levelUpStats;

        Health = GetPawnStats().Health;
        Mana = 0;

        PermanentBuffs = new List<string>();
        Buffs = new Dictionary<string, Buff>();
    }

    public void StartBattle(Pawn pawn)
    {
        Mana = 0;
        Buffs = new Dictionary<string, Buff>();

        foreach (var buff in PermanentBuffs)
        {
            //var buffInstance = Application.Instance.GetManager<ContentManager>().GetBuffFromId(buff).ToDomain(this);
            //buffInstance.Init(this);
            //AddBuff(buffInstance);
        }

        BattleStarted?.Invoke();
        StatsChanged?.Invoke();
    }

    public void FinishBattle()
    {
        RemoveAllBuffs();
        BattleFinished?.Invoke();
    }
    
    public bool AddBuff(Buff newBuff)
    {
        if (Buffs.TryGetValue(newBuff.Id, out var buff))
        {
            buff.TryReapplyBuff();
            return false;
        }

        Buffs.Add(newBuff.Id, newBuff);

        StatsChanged?.Invoke();
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

    public void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff.Id);
        StatsChanged?.Invoke();
    }

    public void ReceiveDamage(DamageDomain damage)
    {
        var reducedDamage = GetPawnStats().GetReducedDamage(damage);
        Health = Mathf.Clamp(Health - reducedDamage, 0, Stats.Health);
        StatsChanged?.Invoke();
    }

    public void ReceiveDamage(int damage)
    {
        Health = Mathf.Clamp(Health - damage, 0, Stats.Health);
        StatsChanged?.Invoke();
    }

    public void ReceiveHeal(int healValue, bool canRevive)
    {
        if (!canRevive && !IsAlive)
            return;

        Health = Mathf.Clamp(Health + healValue, 0, Stats.Health);
        StatsChanged?.Invoke();
    }

    public void EndOfBattleHeal()
    {
        Health = Mathf.Clamp(Health + 15, 0, Stats.Health);
        StatsChanged?.Invoke();
    }

    public void GainMana()
    {
        Mana = Mathf.Clamp(Mana + 10, 0, Stats.Mana);
        StatsChanged?.Invoke();
    }

    public void SpentMana(int manaCost)
    {
        Mana = Mathf.Clamp(Mana - manaCost, 0, Stats.Mana);
        StatsChanged?.Invoke();
    }

    public Stats GetPawnStats()
    {
        var stats = Stats + LevelUpStats.CurrentStats;

        foreach (var (_, buff) in Buffs)
        {
            foreach (var buffComponent in buff)
            {
                if (buffComponent is not StatModifierBuff statModifierBuff)
                    continue;

                stats = statModifierBuff.ProcessStats(stats);
            }
        }

        return stats;
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