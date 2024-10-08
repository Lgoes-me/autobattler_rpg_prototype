using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PawnDomain
{
    public string Id { get; set; }
    public Stats Stats { get; set; }
    private List<AbilityData> Abilities { get; set; }
    public List<AbilityData> SpecialAbilities { get; private set; }
    
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

        HasMana = SpecialAbilities.Count > 0 && Stats.MaxMana > 0;
    }

    public void SetInitiative(float initiative)
    {
        Initiative = initiative;
    }

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Stats.Health = Stats.MaxHealth - pawnInfo.MissingHealth;
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