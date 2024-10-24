using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PawnDomain
{
    public string Id { get; private set; }
    
    public int MaxHealth { get; internal set; }
    public int Health { get; internal set; }

    public int MaxMana { get; internal set; }
    public int Mana { get;  private set; }
    
    public CharacterController Character { get;  private set; }
    public WeaponController Weapon { get;  private set; }
    
    private Stats Stats { get; set; }
    
    private List<AbilityData> Abilities { get; set; }
    public List<AbilityData> SpecialAbilities { get; private set; }
    public List<ArchetypeIdentifier> Archetypes { get; private set; }
    
    public Dictionary<string, Buff> Buffs { get; private set; }
    public AbilityData RequestedSpecialAbility { get; private set; }
    public float Initiative { get; private set; }

    public bool HasMana => SpecialAbilities.Count > 0 && MaxMana > 0;
    public bool IsAlive => Health > 0;

    public delegate void PawnDomainChanged();
    public event PawnDomainChanged LifeChanged;
    public event PawnDomainChanged ManaChanged;
    public event PawnDomainChanged BuffsChanged;

    public PawnDomain(string id,
        int health,
        int mana,
        CharacterController character,
        WeaponController weapon,
        Stats stats,
        List<AbilityData> abilities,
        List<AbilityData> specialAbilities, 
        List<ArchetypeIdentifier> archetypes)
    {
        Id = id;
        MaxHealth = health;
        Health = health;
        MaxMana = mana;
        Mana = 0;
        Character = character;
        Weapon = weapon;
        Stats = stats;

        Abilities = abilities;
        SpecialAbilities = specialAbilities;
        Archetypes = archetypes;
        
        Buffs = new Dictionary<string, Buff>();
        RequestedSpecialAbility = null;
        Initiative = 0;
    }

    public void SetInitiative(float initiative)
    {
        Initiative = initiative;
    }

    public void StartBattle()
    {
        Mana = 0;
        Buffs = new Dictionary<string, Buff>();
        RequestedSpecialAbility = null;
        
        ManaChanged?.Invoke();
        BuffsChanged?.Invoke();
    }
    
    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Health = MaxHealth - pawnInfo.MissingHealth;
        Mana = 0;
        Buffs = new Dictionary<string, Buff>();
        
        LifeChanged?.Invoke();
        ManaChanged?.Invoke();
        BuffsChanged?.Invoke();
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
    
    public void RemoveAllBuffs()
    {
        for (var index = Buffs.Count - 1; index >= 0; index--)
        {
            var buff = Buffs.ElementAt(index);
            RemoveBuff(buff.Value);
        }
    }

    public PawnInfo ResetPawnInfo()
    {
        var pawnInfo = new PawnInfo(Id, 0);
        SetPawnInfo(pawnInfo);
        return pawnInfo;
    }

    public PawnInfo GetPawnInfo()
    {
        return new PawnInfo(Id,  MaxHealth -  Health);
    }

    public Ability GetCurrentAttackIntent(
        PawnController abilityUser, 
        Battle battle,
        bool automaticallyUseSpecials)
    {
        if (RequestedSpecialAbility != null)
        {
            var requestedAbility = RequestedSpecialAbility.ToDomain(abilityUser, true);
            RequestedSpecialAbility = null;
            return requestedAbility;
        }
        
        var abilities = new List<AbilityData>();

        abilities.AddRange(Abilities);

        if (automaticallyUseSpecials)
        {
            var specialAttacks = SpecialAbilities.Where(a => a.ResourceData.GetCost() <=  Mana).ToList();
            abilities.AddRange(specialAttacks);
        }

        return abilities.OrderByDescending(a => a.GetPriority(abilityUser, battle)).First().ToDomain(abilityUser, false);
    }

    public void ReceiveDamage(DamageDomain damage)
    {
        var reducedDamage = GetPawnStats().GetReducedDamage(damage);
        Health = Mathf.Clamp(Health - reducedDamage, 0, MaxHealth);
        LifeChanged?.Invoke();
    }
    
    public void ReceiveHeal(int healValue, bool canRevive)
    {
        if (!canRevive && !IsAlive)
            return;
        
        Health = Mathf.Clamp(Health + healValue, 0, MaxHealth);
        LifeChanged?.Invoke();
    }
    
    public void EndOfBattleHeal()
    {
        Health = Mathf.Clamp(Health + 15, 0, MaxHealth);
        LifeChanged?.Invoke();
    }

    public void GainMana()
    {
        Mana = Mathf.Clamp(Mana + 10, 0,MaxMana);
        ManaChanged?.Invoke();
    }
    
    public void SpentMana(int manaCost)
    {
        Mana = Mathf.Clamp(Mana - manaCost, 0, MaxMana);
        ManaChanged?.Invoke();
    }

    public Stats GetPawnStats()
    {
        var stats = Stats;
        
        foreach (var(_, buff) in Buffs)
        {
            if (buff is not StatModifierBuff statModifierBuff)
                continue;

            stats = statModifierBuff.ProcessStats(stats);
        }
        
        return stats;
    }

    public void DoSpecial(AbilityData ability)
    {
        RequestedSpecialAbility = ability;
    }
}

public enum TeamType
{
    Player,
    Enemies
}