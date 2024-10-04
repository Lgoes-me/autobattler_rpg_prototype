using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public float Initiative { get; private set; }

    private List<AttackData> Attacks { get; set; }
    public List<AttackData> SpecialAttacks { get; private set; }

    public PawnDomain(
        string id,
        int health,
        int mana,
        List<AttackData> attacks,
        List<AttackData> specialAttacks)
    {
        Id = id;
        
        MaxHealth = health;
        Health = health;

        MaxMana = mana;
        Mana = 0;
        
        Initiative = 0;

        Attacks = attacks;
        SpecialAttacks = specialAttacks;
        HasMana = SpecialAttacks.Count > 0 && mana > 0;
    }

    public void SetInitiative(float initiative)
    {
        Initiative = initiative;
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

    public AttackData GetCurrentAttackIntent(bool automaticallyUseSpecials)
    {
        var attacks = new List<AttackData>();

        attacks.AddRange(Attacks);

        if (automaticallyUseSpecials)
        {
            var specialAttacks = SpecialAttacks.Where(a => a.ManaCost <= Mana).ToList();
            attacks.AddRange(specialAttacks);
        }

        return attacks.OrderBy(a => a.GetPriority()).Last();
    }
}

public enum TeamType
{
    Player,
    Enemies
}