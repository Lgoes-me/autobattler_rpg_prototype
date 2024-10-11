using System;
using UnityEngine;

public class AttackValueBuff : Buff
{
    private DamageType Type { get; set; }
    private int Variation { get; set; }

    public AttackValueBuff(DamageType type, int variation, string id, float duration) : base(id, duration)
    {
        Type = type;
        Variation = variation;
    }

    public override void Init(PawnDomain pawn)
    {
        base.Init(pawn);
        switch (Type)
        {
            case DamageType.Physical:
                Pawn.Stats.Strength += Variation;
                break;
            case DamageType.Magical:
                Pawn.Stats.Arcane += Variation;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Deactivate()
    {
        switch (Type)
        {
            case DamageType.Physical:
                Pawn.Stats.Strength -= Variation;
                break;
            case DamageType.Magical:
                Pawn.Stats.Arcane -= Variation;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        base.Deactivate();
    }
    
    public override void TryReapplyBuff()
    {
        base.TryReapplyBuff();

        Duration = Time.time;
    }
}