using UnityEngine;

[System.Serializable]
public class SummonEffectData : EffectData
{
    [field: SerializeField] private EnemyData EnemyData { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new SummonEffect(abilityUser, EnemyData);
    }
}