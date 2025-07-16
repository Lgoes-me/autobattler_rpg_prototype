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
    
    public int EvaluateExperience(int experience, out int remaining)
    {
        var level = 0;
        remaining = 0;
        
        for (int i = 0; i < 10; i++)
        {
            var percent = LevelUpCurve.Evaluate(i / (float) 10);
            var totalExp = MaxLevelStats.GetStat(Stat.ExperienceToLevelUp);
            var levelExp = Mathf.CeilToInt(totalExp * percent);
            
            if (levelExp > experience)
                break;
            
            level = i;
            remaining = experience - levelExp;
        }

        return level;
    }
}