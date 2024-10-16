public class DamageOverTimeEffect : AbilityEffect
{
    private DamageOverTimeBuff DamageOverTimeBuff { get; set; }

    public DamageOverTimeEffect(PawnController abilityUser, DamageOverTimeBuff damageOverTimeBuff) : base(abilityUser)
    {
        DamageOverTimeBuff = damageOverTimeBuff;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (!pawn.IsAlive)
            return;

        DamageOverTimeBuff.SetPawnController(pawnController);
        pawn.AddBuff(DamageOverTimeBuff);
        pawnController.ReceiveBuff(DamageOverTimeBuff);
    }
}
