public class SummonEffect : AbilityEffect
{
    private EnemyInfo EnemyInfo { get; set; }

    public SummonEffect(PawnController abilityUser, EnemyInfo enemyInfo) : base(abilityUser)
    {
        EnemyInfo = enemyInfo;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = EnemyInfo.ToDomain(AbilityUser.Pawn.Team);
        AbilityUser.SummonPawn(pawn);
    }
}