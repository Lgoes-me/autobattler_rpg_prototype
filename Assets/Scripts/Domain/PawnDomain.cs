using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnDomain
{
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    
    public int MaxMana { get; set; }
    public int Mana { get; set; }
    
    private int Size { get; set; }
    public int Initiative { get; private set; }
    
    private List<Attack> Attacks { get; set; }

    public PawnDomain(int health, int size, int initiative, List<Attack> attacks)
    {
        MaxHealth = health;
        Health = health;

        MaxMana = 100;
        Mana = 0;
        
        Initiative = initiative;
        Size = size;

        Attacks = attacks;
    }

    public Attack GetCurrentAttackIntent()
    {
        return Attacks[Random.Range(0, Attacks.Count)];
    }
}

public enum TeamType
{
    Player,
    Enemies
}