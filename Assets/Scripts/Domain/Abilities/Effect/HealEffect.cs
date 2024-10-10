using UnityEngine;

public class HealEffect : AbilityEffect
{
    private float HealValue { get; set; }
    private bool CanRevive { get; set; }

    public HealEffect(PawnController abilityUser, float healValue, bool canRevive) : base(abilityUser)
    {
        HealValue = healValue;
        CanRevive = canRevive;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (!CanRevive && pawn.Stats.Health == 0)
            return;
        
        var heal = (int) (HealValue * AbilityUser.Pawn.Stats.Arcane);
        pawn.Stats.ReceiveHeal(heal, CanRevive);
        pawnController.ReceiveHeal(CanRevive);
    }
}