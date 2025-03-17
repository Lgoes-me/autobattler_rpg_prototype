[System.Serializable]
public class PawnInfo
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int MissingHealth { get; set; }
    
    public PawnInfo(string name)
    {
        Name = name;
        Level = 1;
        MissingHealth = 0;
    }

    public PawnInfo(string name, int level, int missingHealth)
    {
        Name = name;
        Level = level;
        MissingHealth = missingHealth;
    }
    
    //Status, Buff
    //Equips
}
