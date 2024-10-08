using System;
using UnityEngine;

public class Stats
{
    public int MaxHealth { get; }
    public int Health { get; private set; }

    public int MaxMana { get; }
    public int Mana { get;  private set; }

    private int Strength { get; }
    private int Arcane  { get; }
    
    private int PhysicalDefence { get; }
    private int MagicalDefence { get; }
    
    public Stats(int health, int mana, int strength, int arcane, int physicalDefence, int magicalDefence)
    {
        MaxHealth = health;
        Health = health;
        MaxMana = mana;
        Mana = mana;
        Strength = strength;
        Arcane = arcane;
        PhysicalDefence = physicalDefence;
        MagicalDefence = magicalDefence;
    }

    public void ApplyPawnInfo(PawnInfo pawnInfo)
    {
        Health = MaxHealth - pawnInfo.MissingHealth;
    }
    
    public void ReceiveDamage(PawnDomain attacker, Damage damage)
    {
        var damageValue = damage.Type switch
        {
            DamageType.Slash => damage.Multiplier * attacker.Stats.Strength,
            DamageType.Magical => damage.Multiplier * attacker.Stats.Arcane,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var reducedDamage = damage.Type switch
        {
            DamageType.Slash => (int) damageValue - PhysicalDefence,
            DamageType.Magical => (int) damageValue - MagicalDefence,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        if(reducedDamage <= 0)
            return;

        Health = Mathf.Clamp(Health - reducedDamage, 0, MaxHealth);
    }
    
    public void ReceiveHeal(PawnDomain healer, float healValue, bool canRevive)
    {
        if(Health == 0 && !canRevive)
            return;
        
        Health = Mathf.Clamp(Health + (int) (healValue * healer.Stats.Arcane), 0, MaxHealth);
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
}