﻿using UnityEngine;

public class DamageOverTimeBuff : BuffComponent
{
    private DamageDomain Damage { get; set; }
    private float TickRate { get; set; }
    private float LastTick { get; set; }

    public DamageOverTimeBuff(DamageDomain damage, float tickRate)
    {
        Damage = damage;
        TickRate = tickRate;
        LastTick = Time.time;
    }

    public override void OnTick(PawnController focus)
    {
        if (Time.time >= LastTick + TickRate)
        {
            LastTick = Time.time;
            focus.Pawn.ReceiveDamage(Damage);
            focus.ReceiveAttack();
        }
    }
}