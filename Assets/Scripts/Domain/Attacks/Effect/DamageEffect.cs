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
        pawn.Health = Mathf.Clamp(pawn.Health - Damage.Value, 0, pawn.MaxHealth);
        
        pawnController.ReceiveAttack();
    }
}