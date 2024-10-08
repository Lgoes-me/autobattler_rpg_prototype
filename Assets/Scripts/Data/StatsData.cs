using UnityEngine;

[System.Serializable]
public class StatsData
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] private int Mana { get; set; }
    [field: SerializeField] private int Stamina { get; set; }
    
    [field: SerializeField] private int Strength { get; set; }
    [field: SerializeField] private int Dexterity { get; set; }
    [field: SerializeField] private int Inteligence { get; set; }
    
    [field: SerializeField] private int Endurance { get; set; }
    
    [field: SerializeField] private int SlashDefence { get; set; }
    [field: SerializeField] private int MagicalDefence { get; set; }
    [field: SerializeField] private int FireDefence { get; set; }
}