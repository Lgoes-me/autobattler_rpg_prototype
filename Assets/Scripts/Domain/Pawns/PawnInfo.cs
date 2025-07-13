using System.Collections.Generic;

[System.Serializable]
public class PawnInfo
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int MissingHealth { get; set; }
    public PawnStatus Status { get; set; }
    public WeaponType WeaponType { get; set; }
    public string Weapon { get; set; }
    public List<string> Abilities { get; set; }
    public List<string> Buffs { get; set; }
    public List<string> Consumables { get; set; }

    public PawnInfo()
    {
        Name = string.Empty;
        Level = 1;
        Experience = 0;
        MissingHealth = 0;
        WeaponType = WeaponType.None;
        Weapon = string.Empty;
        Abilities = new List<string>();
        Consumables = new List<string>();
        Buffs = new List<string>();
    }
    
    public PawnInfo(
        string name,
        int level,
        int missingHealth,
        PawnStatus status,
        Weapon weapon,
        List<AbilityData> abilities,
        List<ConsumableData> consumables,
        List<string> buffs)
    {
        Name = name;
        Level = level;
        MissingHealth = missingHealth;
        Status = status;
        
        if(weapon != null)
        {
            WeaponType = weapon.Type;
            Weapon = weapon.Id;
        }

        Abilities = new List<string>();
        
        foreach (var ability in abilities)
        {
            Abilities.Add(ability.Id);
        }
        
        Consumables = new List<string>();
        
        foreach (var consumable in consumables)
        {
            Consumables.Add(consumable.Id);
        }
        
        Buffs = buffs;
    }

    public bool CanLevelUp()
    {
        return Level < 10;
    }

    public void LevelUp()
    {
        Level++;
    }

    public void SetWeapon(Weapon weapon)
    {
        Weapon = weapon.Id;
    }
    
    public void SetAbility(AbilityData ability)
    {
        Abilities.Add(ability.Id);
    }
    
    public void SetBuff(BuffData buff)
    {
        Buffs.Add(buff.Id);
    }
    
    public void AddConsumables(ConsumableData consumable)
    {
        Consumables.Add(consumable.Id);
    }

    public void RemoveConsumables(ConsumableData consumable)
    {
        Consumables.Add(consumable.Id);
    }

    public void Update(PawnInfo updatedPawnInfo)
    {
        Level = updatedPawnInfo.Level;
        Experience = updatedPawnInfo.Experience;
        MissingHealth = updatedPawnInfo.MissingHealth;
        Status = updatedPawnInfo.Status;
        WeaponType = updatedPawnInfo.WeaponType;
        Weapon = updatedPawnInfo.Weapon;
        Abilities = updatedPawnInfo.Abilities;
        Buffs = updatedPawnInfo.Buffs;
        Consumables = updatedPawnInfo.Consumables;
    }
}

public enum PawnStatus
{
    Main,
    Locked,
    Unlocked,
    Transient,
    Battle
}