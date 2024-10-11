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

        if (!CanRevive && !pawn.IsAlive)
            return;
        
        var heal = (int) (HealValue * AbilityUser.Pawn.GetPawnStats().Arcane);
        pawn.ReceiveHeal(heal, CanRevive);
        pawnController.ReceiveHeal(CanRevive);
    }
}