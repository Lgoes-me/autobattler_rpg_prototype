using UnityEngine;

[System.Serializable]
public class StatsData
{
    [field: SerializeField] private int Strength { get; set; }
    [field: SerializeField] private int Arcane { get; set; }

    [field: SerializeField] private int PhysicalDefence { get; set; }
    [field: SerializeField] private int MagicalDefence { get; set; }

    public StatsData(int strength, int arcane, int physicalDefence, int magicalDefence)
    {
        Strength = strength;
        Arcane = arcane;
        PhysicalDefence = physicalDefence;
        MagicalDefence = magicalDefence;
    }

    public Stats ToDomain()
    {
        return new Stats(Strength, Arcane, PhysicalDefence, MagicalDefence);
    }
}