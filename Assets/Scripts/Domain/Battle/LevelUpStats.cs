using System.Collections.Generic;
using UnityEngine;

public class LevelUpStats
{
    public Stats CurrentStats { get; private set; }
    private Stats MaxLevelStats { get; }
    private AnimationCurve LevelUpCurve { get; }

    public LevelUpStats(Stats maxLevelStats, AnimationCurve levelUpCurve)
    {
        MaxLevelStats = maxLevelStats;
        LevelUpCurve = levelUpCurve;
        EvaluateLevel(0);
    }

    public void EvaluateLevel(int level)
    {
        var percent = LevelUpCurve.Evaluate(level / (float) 10);
        var newStats = new Dictionary<Stat, int>();

        foreach (var (stat, value) in MaxLevelStats.StatsDictionary)
        {
            newStats.Add(stat, Mathf.CeilToInt(value * percent));
        }
        
        CurrentStats = new Stats(newStats);
    }
}