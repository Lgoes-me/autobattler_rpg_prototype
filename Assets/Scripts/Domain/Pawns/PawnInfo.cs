[System.Serializable]
public class PawnInfo
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int MissingHealth { get; set; }
    public PawnStatus Status { get; set; }
    
    public PawnInfo()
    {
        Name = string.Empty;
        Level = 1;
        MissingHealth = 0;
    }
    
    public PawnInfo(string name)
    {
        Name = name;
        Level = 1;
        MissingHealth = 0;
        Status = PawnStatus.Locked;
    }

    public PawnInfo(string name, int level, int missingHealth, PawnStatus status)
    {
        Name = name;
        Level = level;
        MissingHealth = missingHealth;
        Status = status;
    }
    
    public bool CanLevelUp()
    {
        return Level < 10;
    }
    
    public void LevelUp()
    {
        Level ++;
    }
    public void Update(PawnInfo updatedPawnInfo)
    {
        Level = updatedPawnInfo.Level;
        MissingHealth = updatedPawnInfo.MissingHealth;
    }
    
    
    //Status, Buff
    //Equips
}

public enum PawnStatus
{
    Locked,
    Unlocked,
    Transient
}
