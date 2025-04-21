using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Pawn : BasePawn
{
    public int Level { get; internal set; }
    
    public int Health { get; internal set; }
    public int Mana { get;  private set; }
    
    public int Initiative { get; }
    
    public int VisionRange { get; }
    public int AttackRange { get; }
    public float RangedAttackError { get; }
    
    private Stats Stats { get; }
    private LevelUpStats LevelUpStats { get; }
    public FocusType EnemyFocusPreference { get; } 
    public FocusType AllyFocusPreference { get; }
    
    public List<ArchetypeIdentifier> Archetypes { get; }
    
    public List<string> PermanentBuffs { get; private set; }
    public Dictionary<string, Buff> Buffs { get; private set; }
    public TeamType Team { get; }

    public bool HasMana => Stats.Mana > 0 && Abilities.Any(a => a.ResourceData.GetCost() <=  Mana);
    public bool IsAlive => Health > 0;

    public delegate void PawnDomainChanged();
    public event PawnDomainChanged LostLife;
    public event PawnDomainChanged GainedLife;
    public event PawnDomainChanged LostMana;
    public event PawnDomainChanged GainedMana;
    public event PawnDomainChanged LostBuff;
    public event PawnDomainChanged GainedBuff;

    public delegate void PawnDomainBattleStarted();
    public event PawnDomainBattleStarted BattleStarted;
    
    public delegate void PawnDomainBattleFinished();
    public event PawnDomainBattleFinished BattleFinished;

    public PawnStatus Status { get; private set; }

    public Pawn(int level,
        string id,
        int initiative,
        int visionRange,
        int attackRange,
        float rangedAttackError,
        CharacterController character,
        Stats stats,
        LevelUpStats levelUpStats,
        FocusType enemyFocusPreference,
        FocusType allyFocusPreference,
        List<AbilityData> abilities,
        List<ArchetypeIdentifier> archetypes,
        TeamType team,
        List<CharacterInfo> characterInfos,
        WeaponData weapon,
        WeaponType weaponType) : 
        base(
            id, 
            character, 
            characterInfos, 
            abilities, 
            weapon, 
            weaponType)
    {
        Level = level;
        LevelUpStats = levelUpStats;
        LevelUpStats.EvaluateLevel(Level);
        Stats = stats;
        PermanentBuffs = new List<string>();
        Buffs = new Dictionary<string, Buff>();
        
        Health = GetPawnStats().Health;
        
        Initiative = initiative;
        VisionRange = visionRange;
        AttackRange = attackRange;
        RangedAttackError = rangedAttackError;
        
        EnemyFocusPreference = enemyFocusPreference;
        AllyFocusPreference = allyFocusPreference;
        Archetypes = archetypes;
        Team = team;
    }

    public void StartBattle()
    {
        Mana = 0;
        
        Buffs = new Dictionary<string, Buff>();
        
        foreach (var buff in PermanentBuffs)
        {
            var buffInstance = Application.Instance.GetManager<ContentManager>().GetBuffFromId(buff).ToDomain(this);
            buffInstance.Init(this);
            AddBuff(buffInstance);
        }

        BattleStarted?.Invoke();
        GainedMana?.Invoke();
    }

    public void FinishBattle()
    {
        RemoveAllBuffs();
        
        BattleFinished?.Invoke();
    }
    
    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Level = pawnInfo.Level;
        Health = Stats.Health - pawnInfo.MissingHealth;
        Mana = 0;
        LevelUpStats.EvaluateLevel(Level);
        Status = pawnInfo.Status;

        if (pawnInfo.Weapon != null)
        {
            Weapon = Application.Instance.GetManager<ContentManager>().GetWeaponFromId(pawnInfo.Weapon);
        }

        foreach (var ability in pawnInfo.Abilities)
        {
            if(Abilities.Any(a => a.Id == ability))
                continue;
            
            Abilities.Add(Application.Instance.GetManager<ContentManager>().GetAbilityFromId(ability));
        }

        PermanentBuffs = pawnInfo.Buffs;
    }

    public bool AddBuff(Buff newBuff)
    {
        if (Buffs.TryGetValue(newBuff.Id, out var buff))
        {
            buff.TryReapplyBuff();
            return false;
        }
        
        Buffs.Add(newBuff.Id, newBuff);
        
        GainedBuff?.Invoke();
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
        LostBuff?.Invoke();
    }

    public PawnInfo ResetPawnInfo()
    {
        var pawnInfo = new PawnInfo(Id, Level, 0, Status, Weapon, Abilities);
        SetPawnInfo(pawnInfo);
        return pawnInfo;
    }

    public PawnInfo GetPawnInfo()
    {
        return new PawnInfo(Id, Level, Stats.Health -  Health, Status, Weapon, Abilities);
    }

    public void ReceiveDamage(DamageDomain damage)
    {
        var reducedDamage = GetPawnStats().GetReducedDamage(damage);
        Health = Mathf.Clamp(Health - reducedDamage, 0, Stats.Health);
        LostLife?.Invoke();
    }

    public void ReceiveDamage(int damage)
    {
        Health = Mathf.Clamp(Health - damage, 0, Stats.Health);
        LostLife?.Invoke();
    }

    public void ReceiveHeal(int healValue, bool canRevive)
    {
        if (!canRevive && !IsAlive)
            return;
        
        Health = Mathf.Clamp(Health + healValue, 0, Stats.Health);
        GainedLife?.Invoke();
    }
    
    public void EndOfBattleHeal()
    {
        Health = Mathf.Clamp(Health + 15, 0, Stats.Health);
        GainedLife?.Invoke();
    }

    public void GainMana()
    {
        Mana = Mathf.Clamp(Mana + 10, 0, Stats.Mana);
        GainedMana?.Invoke();
    }
    
    public void SpentMana(int manaCost)
    {
        Mana = Mathf.Clamp(Mana - manaCost, 0, Stats.Mana);
        LostMana?.Invoke();
    }

    public Stats GetPawnStats()
    {        
        var stats = Stats + LevelUpStats.CurrentStats;
        
        foreach (var(_, buff) in Buffs)
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
    
    public Ability GetCurrentAttackIntent(
        PawnController abilityUser, 
        Battle battle)
    {
        var abilities = new List<AbilityData>();
        
        abilities.AddRange(Abilities.Where(a => a.ResourceData.GetCost() <=  Mana).ToList());

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

public enum TeamType
{
    Player,
    Enemies
}