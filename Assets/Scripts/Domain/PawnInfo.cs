[System.Serializable]
public class PawnInfo
{

    public string PawnName { get; set; }
    public int CurrentHealth { get; set; }
    
    public PawnInfo(string name, int health)
    {
        PawnName = name;
        CurrentHealth = health;
    }
    
    //Level
    //Status, Buff, Debuff
    //Equips
}
