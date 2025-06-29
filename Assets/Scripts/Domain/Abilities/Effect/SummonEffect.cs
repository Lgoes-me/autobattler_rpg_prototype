using UnityEngine;

public class SummonEffect : AbilityEffect
{
    private EnemyData EnemyData { get; set; }

    public SummonEffect(PawnController abilityUser, EnemyData enemyData) : base(abilityUser)
    {
        EnemyData = enemyData;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        //TODO REFACTOR ENEMYDATA
        //var pawn = EnemyInfo.ToDomain(AbilityUser.Pawn.Team);
        //AbilityUser.SummonPawn(pawn);
    }
}