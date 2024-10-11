public class SimpleBuffEffect : AbilityEffect
{
    private Buff Buff { get; set; }

    public SimpleBuffEffect(PawnController abilityUser, Buff buff) : base(abilityUser)
    {
        Buff = buff;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (!pawn.IsAlive)
            return;

        pawn.AddBuff(Buff);
        pawnController.ReceiveDebuff(Buff);
    }
}