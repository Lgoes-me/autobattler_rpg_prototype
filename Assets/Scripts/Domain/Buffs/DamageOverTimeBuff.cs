using System;
using UnityEngine;

public class DamageOverTimeBuff : Buff
{
    public Action OnDamageDealt { get; set; }
    private DamageDomain Damage { get; set; }
    private float Duration { get; set; }
    private float TickRate { get; set; }

    private float LastTick { get; set; }
    private float StartingTime { get; set; }

    public DamageOverTimeBuff(DamageDomain damage, float duration, float tickRate)
    {
        Damage = damage;
        Duration = duration;
        TickRate = tickRate;
        LastTick = Time.time;
        StartingTime = Time.time;
    }

    public override void Tick()
    {
        base.Tick();

        if (Time.time >= LastTick + TickRate)
        {
            LastTick = Time.time;
            DebuffedPawn.Stats.ReceiveDamage(Damage);
            OnDamageDealt?.Invoke();
        }

        if (Time.time - StartingTime >= Duration)
        {
            Deactivate();
        }
    }
}