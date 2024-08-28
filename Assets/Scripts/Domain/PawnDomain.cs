[System.Serializable]
public class PawnDomain
{
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    
    public int MaxMana { get; set; }
    public int Mana { get; set; }
    
    public int Attack { get; private set; }
    public int AttackRange { get; private set; }
    public int Size { get; private set; }
    public int Initiative { get; private set; }

    public PawnDomain(int health, int attack, int attackRange, int size, int initiative)
    {
        MaxHealth = health;
        Health = health;

        MaxMana = 100;
        Mana = 0;
        
        Attack = attack;
        AttackRange = attackRange;
        Initiative = initiative;
        Size = size;
    }
}

public enum TeamType
{
    Player,
    Enemies
}