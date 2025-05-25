using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PawnComponent
{
    protected Pawn Pawn { get; private set; }
    public delegate void PawnDomainChanged();

    public void Init(Pawn pawn)
    {
        Pawn = pawn;
    }
}

public class WeaponComponent : PawnComponent
{
    public Weapon Weapon { get; protected set; }
    public WeaponType WeaponType { get; private set; }
    public WeaponController WeaponPrefab { get; private set; }

    public WeaponComponent(Weapon weapon, WeaponType weaponType, WeaponController weaponPrefab)
    {
        Weapon = weapon;
        WeaponType = weaponType;
        WeaponPrefab = weaponPrefab;
    }

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Weapon = Application.Instance.GetManager<ContentManager>().GetWeaponFromId(pawnInfo.Weapon).ToDomain();
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

    public void SetPawnInfo(PawnInfo pawnInfo)
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
    public int Health => GetPawnStats().Health - MissingHealth;
    public int MissingHealth { get; private set; }
    public int Mana { get; private set; }
    public int Level { get; private set; }

    private Stats Stats { get; }
    private LevelUpStats LevelUpStats { get; }

    public List<string> PermanentBuffs { get; private set; }
    public Dictionary<string, Buff> Buffs { get; private set; }

    public bool HasMana => Stats.Mana > 0;
    public bool IsAlive => Health > 0;

    public event PawnDomainChanged LostLife;
    public event PawnDomainChanged GainedLife;
    public event PawnDomainChanged ManaChanged;
    public event PawnDomainChanged BuffsChanged;

    public event PawnDomainChanged BattleStarted;
    public event PawnDomainChanged BattleFinished;

    public StatsComponent(Stats stats, LevelUpStats levelUpStats)
    {
        Stats = stats;
        LevelUpStats = levelUpStats;

        MissingHealth = 0;
        Mana = 0;
        Level = 0;

        PermanentBuffs = new List<string>();
        Buffs = new Dictionary<string, Buff>();
    }

    public void ApplyLevel(int level)
    {
        Level = level;
        LevelUpStats.EvaluateLevel(Level);
    }

    public void StartBattle()
    {
        Mana = 0;
        Buffs = new Dictionary<string, Buff>();

        foreach (var buff in PermanentBuffs)
        {
            var buffInstance = Application.Instance.GetManager<ContentManager>().GetBuffFromId(buff).ToDomain(Pawn, -1);
            buffInstance.Init(Pawn);
            AddBuff(buffInstance);
        }

        BattleStarted?.Invoke();
        ManaChanged?.Invoke();
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

    public void ReceiveDamage(DamageDomain damage)
    {
        var reducedDamage = GetPawnStats().GetReducedDamage(damage);
        
        MissingHealth = Mathf.Clamp(MissingHealth + reducedDamage, 0, GetPawnStats().Health);
        
        LostLife?.Invoke();
    }

    public void ReceiveDamage(int damage)
    {
        MissingHealth = Mathf.Clamp(MissingHealth + damage, 0, GetPawnStats().Health);
        LostLife?.Invoke();
    }

    public void ReceiveHeal(int healValue, bool canRevive)
    {
        if (!canRevive && !IsAlive)
            return;

        MissingHealth = Mathf.Clamp(MissingHealth - healValue, 0, GetPawnStats().Health);
        GainedLife?.Invoke();
    }

    public void EndOfBattleHeal()
    {
        MissingHealth = Mathf.Clamp(MissingHealth - 15, 0, GetPawnStats().Health);
        GainedLife?.Invoke();
    }

    public void GainMana()
    {
        Mana = Mathf.Clamp(Mana + 10, 0, GetPawnStats().Mana);
        ManaChanged?.Invoke();
    }

    public void SpentMana(int manaCost)
    {
        Mana = Mathf.Clamp(Mana - manaCost, 0, GetPawnStats().Mana);
        ManaChanged?.Invoke();
    }

    public Stats GetPawnStats()
    {
        var stats = Stats + LevelUpStats.CurrentStats;

        if(Pawn.TryGetComponent<WeaponComponent>(out var component))
        {
            stats += component.Weapon.Stats;
        }
        
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

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        ApplyLevel(pawnInfo.Level);
        PermanentBuffs = pawnInfo.Buffs;

        MissingHealth = pawnInfo.MissingHealth;
        Mana = 0;
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

public class ConsumableComponent : PawnComponent
{
    public List<ConsumableData> Consumables { get; }
    public event PawnDomainChanged ConsumablesUpdated;

    public ConsumableComponent()
    {
        Consumables = new List<ConsumableData>();
    }

    public void SetPawnInfo(PawnInfo pawnInfo)
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