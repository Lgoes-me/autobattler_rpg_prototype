public class Stats
{
    public int MaxHealth { get; private set; }
    public int Health { get; set; }

    public int MaxMana { get; private set; }
    public int Mana { get; set; }

    public int Strength { get; set; }
    public int Inteligence { get; set; }
    
    public Stats(int health, int mana, int strength, int inteligence)
    {
        MaxHealth = health;
        Health = health;
        MaxMana = mana;
        Mana = mana;
        Strength = strength;
        Inteligence = inteligence;
    }
}