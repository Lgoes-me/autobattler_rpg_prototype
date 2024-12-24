using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pawn : BasePawn
{
    public int MaxHealth { get; internal set; }
    public int Health { get; internal set; }

    public int MaxMana { get; internal set; }
    public int Mana { get;  private set; }
    private Stats Stats { get; }
    private AbilityFocusComponent Focus { get; }
    
    private List<AbilityData> Abilities { get; }
    public List<AbilityData> SpecialAbilities { get; }
    public List<ArchetypeIdentifier> Archetypes { get; }
    
    public Dictionary<string, Buff> Buffs { get; private set; }
    public float Initiative { get; private set; }
    public TeamType Team { get; }

    public bool HasMana => SpecialAbilities.Count > 0 && MaxMana > 0;
    public bool IsAlive => Health > 0;

    public delegate void PawnDomainChanged();
    public event PawnDomainChanged LifeChanged;
    public event PawnDomainChanged ManaChanged;
    public event PawnDomainChanged BuffsChanged;
    
    public delegate void PawnDomainAbilitySelected(Ability ability);
    public event PawnDomainAbilitySelected AbilitySelected;

    public delegate void PawnDomainBattleStarted(Battle battle);
    public event PawnDomainBattleStarted BattleStarted;
    
    public delegate void PawnDomainBattleFinished();
    public event PawnDomainBattleFinished BattleFinished;
    
    public Pawn(string id,
        int health,
        int mana,
        CharacterController character,
        WeaponController weapon,
        Stats stats,
        AbilityFocusComponent focus,
        List<AbilityData> abilities,
        List<AbilityData> specialAbilities, 
        List<ArchetypeIdentifier> archetypes,
        TeamType team,
        List<CharacterInfo> characterInfos) : base(id, character, weapon, characterInfos)
    {
        MaxHealth = health;
        Health = health;
        MaxMana = mana;
        Mana = 0;
        Stats = stats;
        Focus = focus;

        Abilities = abilities;
        SpecialAbilities = specialAbilities;
        Archetypes = archetypes;
        
        Buffs = new Dictionary<string, Buff>();
        Initiative = 0;
        Team = team;
    }

    public void SetInitiative(float initiative)
    {
        Initiative = initiative;
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
        var pawnInfo = new PawnInfo(Id, 0);
        SetPawnInfo(pawnInfo);
        return pawnInfo;
    }

    public PawnInfo GetPawnInfo()
    {
        return new PawnInfo(Id,  MaxHealth -  Health);
    }

    public void ReceiveDamage(DamageDomain damage)
    {
        var reducedDamage = GetPawnStats().GetReducedDamage(damage);
        Health = Mathf.Clamp(Health - reducedDamage, 0, MaxHealth);
        LifeChanged?.Invoke();
    }

    public void ReceiveDamage(int damage)
    {
        Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
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

    public void DoSpecial(AbilityData abilityData, PawnController abilityUser, Battle battle)
    {
        var ability = abilityData.ToDomain(abilityUser, true);
        //ability.ChooseFocus(abilityUser, battle);
        
        AbilitySelected?.Invoke(ability);
    }
    
    public Ability GetCurrentAttackIntent(
        PawnController abilityUser, 
        Battle battle)
    {
        var abilities = new List<AbilityData>();

        abilities.AddRange(Abilities);

        if (Team is TeamType.Enemies)
        {
            var specialAttacks = SpecialAbilities.Where(a => a.ResourceData.GetCost() <=  Mana).ToList();
            abilities.AddRange(specialAttacks);
        }

        return abilities.OrderByDescending(a => a.GetPriority(abilityUser, battle)).First().ToDomain(abilityUser, false);
    }
}

public enum TeamType
{
    Player,
    Enemies
}