using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnDomain
{
    public string Id { get; set; }
    
    public int MaxHealth { get; private set; }
    public int Health { get; set; }

    public int MaxMana { get; private set; }
    public int Mana { get; set; }

    public bool HasMana { get; private set; }
    private int Size { get; set; }
    public int Initiative { get; private set; }

    private List<AttackData> Attacks { get; set; }
    public List<AttackData> SpecialAttacks { get; private set; }

    public PawnDomain(
        string id,
        int health,
        int size,
        int initiative,
        List<AttackData> attacks,
        List<AttackData> specialAttacks)
    {
        Id = id;
        
        MaxHealth = health;
        Health = health;

        MaxMana = 100;
        Mana = 0;

        Size = size;
        Initiative = initiative;

        Attacks = attacks;
        SpecialAttacks = specialAttacks;
        HasMana = SpecialAttacks.Count > 0;
    }

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Health = pawnInfo.CurrentHealth;
    }

    public PawnInfo GetPawnInfo()
    {
        var health = Mathf.Clamp(Health + 15, 0, MaxHealth);
        return new PawnInfo(Id, health);
    }

    public AttackData GetCurrentAttackIntent()
    {
        return Attacks[Random.Range(0, Attacks.Count)];
    }
}

public enum TeamType
{
    Player,
    Enemies
}