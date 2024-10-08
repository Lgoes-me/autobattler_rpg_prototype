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

    public int Strength { get; set; }

    public bool HasMana { get; private set; }
    public float Initiative { get; private set; }

    private List<AbilityData> Abilities { get; set; }
    public List<AbilityData> SpecialAbilities { get; private set; }

    public PawnDomain(
        string id,
        int health,
        int mana,
        int strength,
        List<AbilityData> abilities,
        List<AbilityData> specialAbilities)
    {
        Id = id;

        MaxHealth = health;
        Health = health;

        MaxMana = mana;
        Mana = 0;

        Strength = strength;
        Initiative = 0;

        Abilities = abilities;
        SpecialAbilities = specialAbilities;

        HasMana = SpecialAbilities.Count > 0 && mana > 0;
    }

    public void SetInitiative(float initiative)
    {
        Initiative = initiative;
    }

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Health = MaxHealth - pawnInfo.MissingHealth;
    }

    public PawnInfo GetPawnInfo()
    {
        return new PawnInfo(Id, MaxHealth - Health);
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
            var specialAttacks = SpecialAbilities.Where(a => a.ResourceData.GetCost() <= Mana).ToList();
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