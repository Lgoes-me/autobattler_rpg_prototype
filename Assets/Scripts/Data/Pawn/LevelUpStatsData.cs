using UnityEngine;

[System.Serializable]
public class LevelUpStatsData
{
    [field: SerializeField] private StatsData MaxLevelStatsStats { get; set; }
    [field: SerializeField] private AnimationCurve LevelUpCurve { get; set; }

    public LevelUpStats ToDomain()
    {
        var stats = MaxLevelStatsStats.ToDomain();
        
        return new LevelUpStats(
            stats, 
            LevelUpCurve);
    }
}