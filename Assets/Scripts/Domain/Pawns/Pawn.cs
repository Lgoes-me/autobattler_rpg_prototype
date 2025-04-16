using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
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
    
    private List<AbilityData> Abilities { get; }
    public List<AbilityData> SpecialAbilities { get; }
    public List<ArchetypeIdentifier> Archetypes { get; }
    
    public Dictionary<string, Buff> Buffs { get; private set; }
    public TeamType Team { get; }

    public bool HasMana => SpecialAbilities.Count > 0 && Stats.Mana > 0;
    public bool IsAlive => Health > 0;

    public delegate void PawnDomainChanged();
    public event PawnDomainChanged LifeChanged;
    public event PawnDomainChanged ManaChanged;
    public event PawnDomainChanged BuffsChanged;

    public delegate void PawnDomainBattleStarted(Battle battle);
    public event PawnDomainBattleStarted BattleStarted;
    
    public delegate void PawnDomainBattleFinished();
    public event PawnDomainBattleFinished BattleFinished;

    public PawnStatus Status { get; private set; }
    public WeaponData Weapon { get; private set; }

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
        List<AbilityData> specialAbilities,
        List<ArchetypeIdentifier> archetypes,
        TeamType team,
        List<CharacterInfo> characterInfos,
        WeaponData weapon) : base(id, character, characterInfos)
    {
        Level = level;
        LevelUpStats = levelUpStats;
        LevelUpStats.EvaluateLevel(Level);
        Stats = stats;
        Buffs = new Dictionary<string, Buff>();
        
        Health = GetPawnStats().Health;
        
        Initiative = initiative;
        VisionRange = visionRange;
        AttackRange = attackRange;
        RangedAttackError = rangedAttackError;
        
        EnemyFocusPreference = enemyFocusPreference;
        AllyFocusPreference = allyFocusPreference;
        Abilities = abilities;
        SpecialAbilities = specialAbilities;
        Archetypes = archetypes;
        Team = team;
        Weapon = weapon;
    }

    public void StartBattle(Battle battle)
    {
        Mana = 0;
        Buffs = new Dictionary<string, Buff>();
        
        BattleStarted?.Invoke(battle);
        ManaChanged?.Invoke();
        BuffsChanged?.Invoke();
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

        if (!string.IsNullOrWhiteSpace(pawnInfo.Weapon))
        {
            Weapon = Application.Instance.GetManager<ContentManager>().GetWeaponFromId(pawnInfo.Weapon);
        }
        
        Buffs = new Dictionary<string, Buff>();
    }

    public void AddBuff(Buff newBuff)
    {
        if (Buffs.TryGetValue(newBuff.Id, out var buff))
        {
            buff.TryReapplyBuff();
            return;
        }
        
        newBuff.Init(this);
        Buffs.Add(newBuff.Id, newBuff);
        
        BuffsChanged?.Invoke();
    }

    public void TickAllBuffs()
    {
        for (var index = Buffs.Count - 1; index >= 0; index--)
        {
            var buff = Buffs.ElementAt(index);
            buff.Value.Tick();
        }
    }
    
    public void RemoveBuff(Buff buff)
    {
        buff.Deactivate();
        Buffs.Remove(buff.Id);
        
        BuffsChanged?.Invoke();
    }
    
    private void RemoveAllBuffs()
    {
        for (var index = Buffs.Count - 1; index >= 0; index--)
        {
            var buff = Buffs.ElementAt(index);
            RemoveBuff(buff.Value);
        }
    }

    public PawnInfo ResetPawnInfo()
    {
        var pawnInfo = new PawnInfo(Id, Level, 0, Status);
        SetPawnInfo(pawnInfo);
        return pawnInfo;
    }

    public PawnInfo GetPawnInfo()
    {
        return new PawnInfo(Id, Level, Stats.Health -  Health, Status);
    }

    public void ReceiveDamage(DamageDomain damage)
    {
        var reducedDamage = GetPawnStats().GetReducedDamage(damage);
        Health = Mathf.Clamp(Health - reducedDamage, 0, Stats.Health);
        LifeChanged?.Invoke();
    }

    public void ReceiveDamage(int damage)
    {
        Health = Mathf.Clamp(Health - damage, 0, Stats.Health);
        LifeChanged?.Invoke();
    }

    public void ReceiveHeal(int healValue, bool canRevive)
    {
        if (!canRevive && !IsAlive)
            return;
        
        Health = Mathf.Clamp(Health + healValue, 0, Stats.Health);
        LifeChanged?.Invoke();
    }
    
    public void EndOfBattleHeal()
    {
        Health = Mathf.Clamp(Health + 15, 0, Stats.Health);
        LifeChanged?.Invoke();
    }

    public void GainMana()
    {
        Mana = Mathf.Clamp(Mana + 10, 0, Stats.Mana);
        ManaChanged?.Invoke();
    }
    
    public void SpentMana(int manaCost)
    {
        Mana = Mathf.Clamp(Mana - manaCost, 0, Stats.Mana);
        ManaChanged?.Invoke();
    }

    public Stats GetPawnStats()
    {        
        var stats = Stats + LevelUpStats.CurrentStats;
        
        foreach (var(_, buff) in Buffs)
        {
            if (buff is not StatModifierBuff statModifierBuff)
                continue;

            stats = statModifierBuff.ProcessStats(stats);
        }
        
        return stats;
    }
    
    public Ability GetCurrentAttackIntent(
        PawnController abilityUser, 
        Battle battle)
    {
        var abilities = new List<AbilityData>();

        abilities.AddRange(Abilities);

        var specialAttacks = SpecialAbilities.Where(a => a.ResourceData.GetCost() <=  Mana).ToList();
        abilities.AddRange(specialAttacks);

        var lowestValue = abilities
            .ToDictionary(a => a, a => a.GetPriority(abilityUser, battle))
            .OrderBy(p => p.Value).First().Value;
            
        var selected = abilities
            .ToDictionary(a => a, a => a.GetPriority(abilityUser, battle))
            .OrderBy(p => p.Value)
            .Where(k => k.Value == lowestValue)
            .Select(p => p.Key)
            .OrderBy(a => Guid.NewGuid())
            .First();

        return selected.ToDomain(abilityUser);
    }
}

public enum TeamType
{
    Player,
    Enemies
}