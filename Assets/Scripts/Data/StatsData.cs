using UnityEngine;

[System.Serializable]
public class StatsData
{
    [field: SerializeField] private int Health { get; set; }
    [field: SerializeField] private int Mana { get; set; }
    
    [field: SerializeField] private int Strength { get; set; }
    [field: SerializeField] private int Arcane { get; set; }
    
    
    [field: SerializeField] private int PhysicalDefence { get; set; }
    [field: SerializeField] private int MagicalDefence { get; set; }

    public Stats ToDomain()
    {
        return new Stats(Health, Mana, Strength, Arcane, PhysicalDefence, MagicalDefence);
    }
}