using System;
using UnityEngine;

public class DamageOverTimeBuff : Buff
{
    public Action OnDamageDealt { get; set; }
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
            Pawn.Stats.ReceiveDamage(Damage);
            OnDamageDealt?.Invoke();
        }

        base.Tick();
    }

    public override void TryReapplyBuff()
    {
        base.TryReapplyBuff();

        Duration = Time.time;
    }
}