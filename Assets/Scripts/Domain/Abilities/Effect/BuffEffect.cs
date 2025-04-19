public class BuffEffect : AbilityEffect
{
    private Buff Buff { get; set; }

    public BuffEffect(PawnController abilityUser, Buff buff) : base(abilityUser)
    {
        Buff = buff;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (!pawn.IsAlive)
            return;

        if (pawn.AddBuff(Buff))
        {
            Buff.Init(pawnController);
        }
    }    
}