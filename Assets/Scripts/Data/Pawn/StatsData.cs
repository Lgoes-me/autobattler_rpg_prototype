using UnityEngine;

[System.Serializable]
public class StatsData
{
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public int Mana { get; set; }
    
    [field: SerializeField] public int Strength { get; set; }
    [field: SerializeField] public int Arcane { get; set; }

    [field: SerializeField] public int PhysicalDefence { get; set; }
    [field: SerializeField] public int MagicalDefence { get; set; }


    public Stats ToDomain()
    {
        return new Stats(
            Health, 
            Mana, 
            Strength, 
            Arcane, 
            PhysicalDefence, 
            MagicalDefence);
    }
}