﻿using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PawnDomain
{
    public string Id { get; private set; }
    public Stats Stats { get; private set; }
    private List<AbilityData> Abilities { get; set; }
    public List<AbilityData> SpecialAbilities { get; private set; }
    public List<Buff> Buffs { get; private set; }
    
    public bool HasMana { get; private set; }
    public float Initiative { get; private set; }

    public PawnDomain(
        string id,
        Stats stats,
        List<AbilityData> abilities,
        List<AbilityData> specialAbilities)
    {
        Id = id;
        Stats = stats;
        Initiative = 0;

        Abilities = abilities;
        SpecialAbilities = specialAbilities;
        Buffs = new List<Buff>();

        HasMana = SpecialAbilities.Count > 0 && Stats.MaxMana > 0;
    }

    public void SetInitiative(float initiative)
    {
        Initiative = initiative;
    }

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Stats.ApplyPawnInfo(pawnInfo);
    }

    public void AddBuff(Buff newBuff)
    {
        newBuff.Init(this);
        Buffs.Add(newBuff);
    }

    public void TickAllBuffs()
    {
        for (var index = Buffs.Count - 1; index >= 0; index--)
        {
            var buff = Buffs[index];
            buff.Tick();
        }
    }
    
    public void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff);
    }

    public PawnInfo GetPawnInfo()
    {
        return new PawnInfo(Id,  Stats.MaxHealth -  Stats.Health);
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
            var specialAttacks = SpecialAbilities.Where(a => a.ResourceData.GetCost() <=  Stats.Mana).ToList();
            abilities.AddRange(specialAttacks);
        }

        return abilities.OrderBy(a => a.GetPriority(abilityUser, battle)).Last();
    }
}

public enum TeamType
{
    Player,
    Enemies
}