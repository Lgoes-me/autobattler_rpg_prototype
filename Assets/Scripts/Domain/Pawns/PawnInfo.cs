[System.Serializable]
public class PawnInfo
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int MissingHealth { get; set; }
    public PawnStatus Status { get; set; }
    public string Weapon { get; set; }

    public PawnInfo()
    {
        Name = string.Empty;
        Level = 1;
        MissingHealth = 0;
        Weapon = string.Empty;
    }

    public PawnInfo(string name, PawnStatus status)
    {
        Name = name;
        Level = 1;
        MissingHealth = 0;
        Status = status;
        Weapon = string.Empty;
    }

    public PawnInfo(string name, int level, int missingHealth, PawnStatus status)
    {
        Name = name;
        Level = level;
        MissingHealth = missingHealth;
        Status = status;
        Weapon = string.Empty;
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
        Weapon = weapon.Id;
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