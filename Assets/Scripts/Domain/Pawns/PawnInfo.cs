[System.Serializable]
public class PawnInfo
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int MissingHealth { get; set; }
    public PawnStatus Status { get; set; }
    public WeaponInfo Weapon { get; set; }

    public PawnInfo()
    {
        Name = string.Empty;
        Level = 1;
        MissingHealth = 0;
        Weapon = null;
    }

    public PawnInfo(string name, PawnStatus status)
    {
        Name = name;
        Level = 1;
        MissingHealth = 0;
        Status = status;
        Weapon = null;
    }

    public PawnInfo(string name, int level, int missingHealth, PawnStatus status, WeaponData weapon)
    {
        Name = name;
        Level = level;
        MissingHealth = missingHealth;
        Status = status;
        
        if(weapon != null)
        {
            Weapon = new WeaponInfo(weapon);
        }
    }

    public bool CanLevelUp()
    {
        return Level < 10;
    }

    public void LevelUp()
    {
        Level++;
    }

    public void SetWeapon(WeaponData weapon)
    {
        Weapon = new WeaponInfo(weapon);
    }
    
    public void Update(PawnInfo updatedPawnInfo)
    {
        Level = updatedPawnInfo.Level;
        MissingHealth = updatedPawnInfo.MissingHealth;
    }


    //Status, Buff
}

public enum PawnStatus
{
    Main,
    Locked,
    Unlocked,
    Transient
}