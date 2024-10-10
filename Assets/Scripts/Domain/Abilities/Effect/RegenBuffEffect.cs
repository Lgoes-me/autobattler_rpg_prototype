public class RegenBuffEffect : AbilityEffect
{
    private RegenBuff RegenBuff { get; set; }

    public RegenBuffEffect(PawnController abilityUser, RegenBuff regenBuff) : base(abilityUser)
    {
        RegenBuff = regenBuff;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (pawn.Stats.Health == 0)
            return;

        RegenBuff.SetPawnController(pawnController);
        pawn.AddBuff(RegenBuff);
        pawnController.ReceiveDebuff(RegenBuff);
    }
}
