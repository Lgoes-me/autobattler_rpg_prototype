using System.Collections.Generic;

[System.Serializable]
public class PawnInfo
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int MissingHealth { get; set; }
    public PawnStatus Status { get; set; }
    public string Weapon { get; set; }
    public List<string> Abilities { get; set; }
    public List<string> Buffs { get; set; }

    public PawnInfo()
    {
        Name = string.Empty;
        Level = 1;
        MissingHealth = 0;
        Weapon = string.Empty;
        Abilities = new List<string>();
        Buffs = new List<string>();
    }
    
    public PawnInfo(
        string name, 
        int level, 
        int missingHealth,
        PawnStatus status, 
        Weapon weapon, 
        List<AbilityData> abilities)
    {
        Name = name;
        Level = level;
        MissingHealth = missingHealth;
        Status = status;
        
        if(weapon != null)
        {
            Weapon = weapon.Id;
        }

        Abilities = new List<string>();
        
        foreach (var ability in abilities)
        {
            Abilities.Add(ability.Id);
        }
        
        Buffs = new List<string>();
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
    
    public void Update(PawnInfo updatedPawnInfo)
    {
        Level = updatedPawnInfo.Level;
        MissingHealth = updatedPawnInfo.MissingHealth;
        Status = updatedPawnInfo.Status;
        Weapon = updatedPawnInfo.Weapon;
        Abilities = updatedPawnInfo.Abilities;
        Buffs = updatedPawnInfo.Buffs;
    }

    //Status
}

public enum PawnStatus
{
    Main,
    Locked,
    Unlocked,
    Transient,
    Enemy
}