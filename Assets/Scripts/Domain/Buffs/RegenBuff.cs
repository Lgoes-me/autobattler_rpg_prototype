using UnityEngine;

public class RegenBuff : Buff
{
    private int Regen { get; set; }
    private float TickRate { get; set; }

    private float LastTick { get; set; }

    public RegenBuff(int regen, float tickRate, string id, float duration) : base(id, duration)
    {
        Regen = regen;
        TickRate = tickRate;
        LastTick = Time.time;
    }

    public override bool Tick()
    {
        if (Time.time >= LastTick + TickRate)
        {
            LastTick = Time.time;
            PawnController.Pawn.ReceiveHeal(Regen, false);
            PawnController.ReceiveHeal(false);
        }

        return base.Tick();
    }

    public override void TryReapplyBuff()
    {
        base.TryReapplyBuff();
        Duration = Time.time;
    }
}
