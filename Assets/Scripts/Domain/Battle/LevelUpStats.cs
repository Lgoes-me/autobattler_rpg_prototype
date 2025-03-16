using UnityEngine;

public class LevelUpStats
{
    public Stats CurrentStats { get; private set; }
    private Stats MaxLevelStats { get; }
    private AnimationCurve LevelUpCurve { get; }

    public LevelUpStats(Stats maxLevelStats, AnimationCurve levelUpCurve)
    {
        CurrentStats = null;
        MaxLevelStats = maxLevelStats;
        LevelUpCurve = levelUpCurve;
    }

    public void EvaluateLevel(int level)
    {
        var value = LevelUpCurve.Evaluate(level / (float) 10);

        CurrentStats = new Stats(
            Mathf.CeilToInt(MaxLevelStats.Health * value),
            Mathf.CeilToInt(MaxLevelStats.Mana * value),
            Mathf.CeilToInt(MaxLevelStats.Strength * value),
            Mathf.CeilToInt(MaxLevelStats.Arcane * value),
            Mathf.CeilToInt(MaxLevelStats.PhysicalDefence * value),
            Mathf.CeilToInt(MaxLevelStats.MagicalDefence * value));
    }
}