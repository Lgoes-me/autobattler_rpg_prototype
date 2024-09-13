using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnDomain
{
    public int MaxHealth { get; private set; }
    public int Health { get; set; }

    public int MaxMana { get; private set; }
    public int Mana { get; set; }

    public bool HasMana { get; private set; }
    private int Size { get; set; }
    public int Initiative { get; private set; }

    private List<AttackData> Attacks { get; set; }
    private List<AttackData> SpecialAttacks { get; set; }

    public PawnDomain(
        int health,
        int size,
        int initiative,
        bool hasMana,
        List<AttackData> attacks,
        List<AttackData> specialAttacks)
    {
        MaxHealth = health;
        Health = health;

        MaxMana = 100;
        Mana = 0;

        Size = size;
        Initiative = initiative;
        HasMana = hasMana;

        Attacks = attacks;
        SpecialAttacks = specialAttacks;
    }

    public AttackData GetCurrentAttackIntent(bool specialAttackRequested)
    {
        if (specialAttackRequested)
        {
            return SpecialAttacks[Random.Range(0, SpecialAttacks.Count)];
        }

        return Attacks[Random.Range(0, Attacks.Count)];
    }
}

public enum TeamType
{
    Player,
    Enemies
}