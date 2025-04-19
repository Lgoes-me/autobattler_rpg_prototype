using UnityEngine;

public class DamageOverTimeBuff : Buff
{
    private DamageDomain Damage { get; set; }
    private float TickRate { get; set; }

    private float LastTick { get; set; }

    public DamageOverTimeBuff(DamageDomain damage, float tickRate, string id, float duration) : base(id, duration)
    {
        Damage = damage;
        TickRate = tickRate;
        LastTick = Time.time;
    }

    public override bool Tick()
    {
        if (Time.time >= LastTick + TickRate)
        {
            LastTick = Time.time;
            Focus.Pawn.ReceiveDamage(Damage);
            Focus.ReceiveAttack();
        }

        return base.Tick();
    }

    public override void TryReapplyBuff()
    {
        base.TryReapplyBuff();
        Duration = Time.time;
    }
}