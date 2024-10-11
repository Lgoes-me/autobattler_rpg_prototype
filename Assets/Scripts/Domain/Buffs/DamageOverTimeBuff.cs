using UnityEngine;

public class DamageOverTimeBuff : Buff
{
    private PawnController PawnController { get; set; }
    private DamageDomain Damage { get; set; }
    private float TickRate { get; set; }

    private float LastTick { get; set; }

    public DamageOverTimeBuff(DamageDomain damage, float tickRate, string id, float duration) : base(id, duration)
    {
        Damage = damage;
        TickRate = tickRate;
        LastTick = Time.time;
    }

    public override void Tick()
    {
        if (Time.time >= LastTick + TickRate)
        {
            LastTick = Time.time;
            Pawn.ReceiveDamage(Damage);
            PawnController.ReceiveAttack();
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