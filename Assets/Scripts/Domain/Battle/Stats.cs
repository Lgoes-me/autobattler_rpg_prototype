using System;
using UnityEngine;

[Serializable]
public class Stats
{
    public int Health { get; internal set; }
    public int Mana { get; internal set; }
    
    public int Strength { get; private set; }
    public int Arcane { get; private set; }
    
    public int PhysicalDefence { get; private set; }
    public int MagicalDefence { get; private set; }
    
    public Stats(int health, int mana,int strength, int arcane, int physicalDefence, int magicalDefence)
    {
        Health = health;
        Mana = mana;
        Strength = strength;
        Arcane = arcane;
        PhysicalDefence = physicalDefence;
        MagicalDefence = magicalDefence;
    }

    public int GetReducedDamage(DamageDomain damage)
    {
        var damageValue = damage.CalculateDamageValue();

        var reducedDamage = damage.Type switch
        {
            DamageType.Physical => damageValue - (damageValue * PhysicalDefence * 10/100),
            DamageType.Magical => damageValue - (damageValue * MagicalDefence * 10/100),
            _ => throw new ArgumentOutOfRangeException()
        };

        return reducedDamage >= 0 ? reducedDamage : 0;
    }

    public static Stats operator +(Stats a, Stats b)
    {
        return new Stats(
            a.Health + b.Health,
            a.Mana + b.Mana,
            a.Strength + b.Strength,
            a.Arcane + b.Arcane,
            a.PhysicalDefence + b.PhysicalDefence,
            a.MagicalDefence + b.MagicalDefence);
    }

    public void Print()
    {
        Debug.Log($"Health {Health}");
        Debug.Log($"Mana {Mana}");
        Debug.Log($"Strength {Strength}");
        Debug.Log($"Arcane {Arcane}");
        Debug.Log($"PhysicalDefence {PhysicalDefence}");
        Debug.Log($"MagicalDefence {MagicalDefence}");
    }
}