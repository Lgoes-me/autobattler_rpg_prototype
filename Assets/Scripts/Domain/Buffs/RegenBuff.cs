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
    
    public override void OnTick(PawnController focus)
    {
        if (Time.time >= LastTick + TickRate)
        {
            LastTick = Time.time;
            focus.Pawn.ReceiveHeal(Regen, false);
            focus.ReceiveHeal(false);
        }
    }
}
