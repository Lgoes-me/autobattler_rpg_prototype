public class BuffEffect : AbilityEffect
{
    private Buff Buff { get; set; }

    public BuffEffect(PawnController abilityUser, Buff buff) : base(abilityUser)
    {
        Buff = buff;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var stats = focus.Pawn.GetComponent<StatsComponent>();
        var buffs = focus.Pawn.GetComponent<PawnBuffsComponent>();
        
        if (!stats.IsAlive)
            return;

        if (buffs.AddBuff(Buff))
        {
            Buff.Init(focus.Pawn);
        }
    }    
}