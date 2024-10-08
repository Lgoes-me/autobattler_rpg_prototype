using System;
using UnityEngine;

public class DamageEffect : AbilityEffect
{
    private Damage Damage { get; set; }

    public DamageEffect(PawnController abilityUser, Damage damage) : base(abilityUser)
    {
        Damage = damage;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (pawn.Stats.Health == 0)
            return;

        pawn.Stats.ReceiveDamage(AbilityUser.Pawn, Damage);
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