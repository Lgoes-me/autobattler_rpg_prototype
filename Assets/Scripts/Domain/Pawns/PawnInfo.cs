[System.Serializable]
public class PawnInfo
{
    public int Level { get; set; }
    public int MissingHealth { get; set; }
    
    public PawnInfo()
    {
        Level = 1;
        MissingHealth = 0;
    }

    public PawnInfo(int level, int missingHealth)
    {
        Level = level;
        MissingHealth = missingHealth;
    }
    
    //Status, Buff
    //Equips
}
