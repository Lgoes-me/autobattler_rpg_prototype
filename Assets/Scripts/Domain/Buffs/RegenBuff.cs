using UnityEngine;

public class RegenBuff : BuffComponent
{
    private int Regen { get; set; }
    private float TickRate { get; set; }

    private float LastTick { get; set; }

    public RegenBuff(int regen, float tickRate)
    {
        Regen = regen;
        TickRate = tickRate;
        LastTick = Time.time;
    }
    
    public override void OnTick(Pawn focus)
    {
        if (Time.time >= LastTick + TickRate)
        {
            LastTick = Time.time;
            focus.GetComponent<StatsComponent>().ReceiveHeal(Regen, false);
        }
    }
}
