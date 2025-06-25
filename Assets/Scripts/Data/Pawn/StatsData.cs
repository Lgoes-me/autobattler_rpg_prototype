using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class StatsData : IEnumerable
{
    [field: SerializeField] public List<StatData> Stats { get; set; }

    public Stats ToDomain()
    {
        var stats = Stats.ToDictionary(s => s.Stat, s => s.Value);
        return new Stats(stats);
    }

    public void Add(StatData stat)
    {
        Stats ??= new List<StatData>();
        Stats.Add(stat);
    }

    public IEnumerator GetEnumerator()
    {
        return Stats.GetEnumerator();
    }
}

[System.Serializable]
public class StatData
{
    [field: SerializeField] public Stat Stat { get; set; }
    [field: SerializeField] public int Value { get; set; }
    
    public StatData(Stat stat, int value)
    {
        Stat = stat;
        Value = value;
    }
}