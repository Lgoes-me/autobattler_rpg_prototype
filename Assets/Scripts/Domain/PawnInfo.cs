[System.Serializable]
public class PawnInfo
{
    public string PawnName { get; set; }
    public int MissingHealth { get; set; }
    
    public PawnInfo(string pawnName, int missingHealth)
    {
        PawnName = pawnName;
        MissingHealth = missingHealth;
    }
    
    //Level
    //Status, Buff, Debuff
    //Equips
}
