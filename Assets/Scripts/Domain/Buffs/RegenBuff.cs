using UnityEngine;

public class RegenBuff : Buff
{
    private PawnController PawnController { get; set; }
    private int Regen { get; set; }
    private float TickRate { get; set; }

    private float LastTick { get; set; }

    public RegenBuff(int regen, float tickRate, string id, float duration) : base(id, duration)
    {
        Regen = regen;
        TickRate = tickRate;
        LastTick = Time.time;
    }

    public override void Tick()
    {
        if (Time.time >= LastTick + TickRate)
        {
            LastTick = Time.time;
            Pawn.Stats.ReceiveHeal(Regen, false);
            PawnController.ReceiveHeal(false);
        }

        base.Tick();
    }

    public override void TryReapplyBuff()
    {
        base.TryReapplyBuff();
        Duration = Time.time;
    }
    
    public void SetPawnController(PawnController pawnController)
    {
        PawnController = pawnController;
    }
}
