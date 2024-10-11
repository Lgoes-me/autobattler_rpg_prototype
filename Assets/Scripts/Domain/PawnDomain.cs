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
    
    private Stats Stats { get; set; }
    
    private List<AbilityData> Abilities { get; set; }
    public List<AbilityData> SpecialAbilities { get; private set; }
    
    public Dictionary<string, Buff> Buffs { get; private set; }

    public bool HasMana => SpecialAbilities.Count > 0 && MaxMana > 0;
    public bool IsAlive => Health > 0;
    public float Initiative { get; private set; }

    public PawnDomain(
        string id,
        int health,
        int mana,
        Stats stats,
        List<AbilityData> abilities,
        List<AbilityData> specialAbilities)
    {
        Id = id;
        MaxHealth = health;
        Health = health;
        MaxMana = mana;
        Mana = 0;
        Stats = stats;
        Initiative = 0;

        Abilities = abilities;
        SpecialAbilities = specialAbilities;
        Buffs = new Dictionary<string, Buff>();
    }

    public void SetInitiative(float initiative)
    {
        Initiative = initiative;
    }

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Health = MaxHealth - pawnInfo.MissingHealth;
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
    }
    
    public void RemoveAllBuffs()
    {
        for (var index = Buffs.Count - 1; index >= 0; index--)
        {
            var buff = Buffs.ElementAt(index);
            RemoveBuff(buff.Value);
        }
    }

    public PawnInfo GetPawnInfo()
    {
        return new PawnInfo(Id,  MaxHealth -  Health);
    }

    public AbilityData GetCurrentAttackIntent(
        PawnController abilityUser, 
        Battle battle,
        bool automaticallyUseSpecials)
    {
        var abilities = new List<AbilityData>();

        abilities.AddRange(Abilities);

        if (automaticallyUseSpecials)
        {
            var specialAttacks = SpecialAbilities.Where(a => a.ResourceData.GetCost() <=  Mana).ToList();
            abilities.AddRange(specialAttacks);
        }

        return abilities.OrderByDescending(a => a.GetPriority(abilityUser, battle)).First();
    }

    public void ReceiveDamage(DamageDomain damage)
    {
        var reducedDamage = GetPawnStats().GetReducedDamage(damage);
        Health = Mathf.Clamp(Health - reducedDamage, 0, MaxHealth);
    }
    
    public void ReceiveHeal(int healValue, bool canRevive)
    {
        if (!canRevive && !IsAlive)
            return;
        
        Health = Mathf.Clamp(Health + healValue, 0, MaxHealth);
    }
    
    public void EndOfBattleHeal()
    {
        Health = Mathf.Clamp(Health + 15, 0, MaxHealth);
    }

    public void GainMana()
    {
        Mana = Mathf.Clamp(Mana + 10, 0,MaxMana);
    }
    
    public void SpentMana(int manaCost)
    {
        Mana = Mathf.Clamp(Mana - manaCost, 0, MaxMana);
    }

    public Stats GetPawnStats()
    {
        var stats = Stats;
        
        foreach (var(key, buff) in Buffs)
        {
            if (buff is not StatModifierBuff statModifierBuff)
                continue;

            stats = statModifierBuff.ProcessStats(stats);
        }
        
        return stats;
    }
}

public enum TeamType
{
    Player,
    Enemies
}