using System;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public int MaxHealth { get; }
    public int Health { get; private set; }

    public int MaxMana { get; }
    public int Mana { get;  private set; }

    public int Strength { get; set; }
    public int Arcane { get; set; }
    
    public int PhysicalDefence { get; private set; }
    public int MagicalDefence { get; private set; }
    
    public Stats(int health, int mana, int strength, int arcane, int physicalDefence, int magicalDefence)
    {
        MaxHealth = health;
        Health = health;
        MaxMana = mana;
        Mana = 0;
        Strength = strength;
        Arcane = arcane;
        PhysicalDefence = physicalDefence;
        MagicalDefence = magicalDefence;
    }

    public void ApplyPawnInfo(PawnInfo pawnInfo)
    {
        Health = MaxHealth - pawnInfo.MissingHealth;
    }
    
    public void ReceiveDamage(DamageDomain damage)
    {
        var damageValue = damage.CalculateDamageValue();
        
        var reducedDamage = damage.Type switch
        {
            DamageType.Physical => damageValue - (damageValue * PhysicalDefence * 10/100),
            DamageType.Magical => damageValue - (damageValue * MagicalDefence * 10/100),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        if(reducedDamage <= 0)
            return;

        Health = Mathf.Clamp(Health - reducedDamage, 0, MaxHealth);
    }
    
    public void ReceiveHeal(int healValue, bool canRevive)
    {
        if(Health == 0 && !canRevive)
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
}