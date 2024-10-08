using System;
using UnityEngine;

public class DamageEffect : AbilityEffect
{
    private Damage Damage { get; set; }

    public DamageEffect(Damage damage)
    {
        Damage = damage;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (pawn.Stats.Health == 0)
            return;

        var damage = Damage.Type switch
        {
            DamageType.Slash => Damage.Multiplier * pawn.Stats.Strength,
            DamageType.Magical => Damage.Multiplier * pawn.Stats.Inteligence,
            _ => throw new ArgumentOutOfRangeException()
        };

        pawn.Stats.Health = Mathf.Clamp(pawn.Stats.Health - (int) damage, 0, pawn.Stats.MaxHealth);

        pawnController.ReceiveAttack();
    }
}

[System.Serializable]
public class Damage
{
    [field: SerializeField] public float Multiplier { get; set; }
    [field: SerializeField] public DamageType Type { get; set; }
}

public enum DamageType
{
    Slash = 1,
    Magical = 2,
}