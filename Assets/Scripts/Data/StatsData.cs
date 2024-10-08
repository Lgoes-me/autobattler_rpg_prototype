using UnityEngine;

[System.Serializable]
public class StatsData
{
    [field: SerializeField] private int Health { get; set; }
    [field: SerializeField] private int Mana { get; set; }
    
    [field: SerializeField] private int Strength { get; set; }
    [field: SerializeField] private int Inteligence { get; set; }
    
    [field: SerializeField] private int Endurance { get; set; }
    
    [field: SerializeField] private int SlashDefence { get; set; }
    [field: SerializeField] private int MagicalDefence { get; set; }

    public Stats ToDomain()
    {
        return new Stats(Health, Mana, Strength, Inteligence);
    }
}