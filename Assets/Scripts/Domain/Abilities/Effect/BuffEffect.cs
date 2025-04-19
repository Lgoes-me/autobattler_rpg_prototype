public class BuffEffect : AbilityEffect
{
    private Buff Buff { get; set; }

    public BuffEffect(PawnController abilityUser, Buff buff) : base(abilityUser)
    {
        Buff = buff;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        if (!focus.Pawn.IsAlive)
            return;

        if (focus.Pawn.AddBuff(Buff))
        {
            Buff.Init(focus);
        }
    }    
}