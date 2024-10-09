public class DamageOverTimeBuff : Buff
{
    private DamageData DamageData { get; set; }
    private float Duration { get; set; }
    private float TickRate { get; set; }
    
    private float LastTick { get; set; }

    public DamageOverTimeBuff(DamageData damageData, float duration, float tickRate)
    {
        DamageData = damageData;
        Duration = duration;
        TickRate = tickRate;
    }

    public override void Tick()
    {
        base.Tick();
    }
}