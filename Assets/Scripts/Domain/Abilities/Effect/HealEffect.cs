using UnityEngine;
public class HealEffect : AbilityEffect
{
    private int HealValue { get; set; }
    private bool CanRevive { get; set; }

    public HealEffect(int healValue, bool canRevive)
    {
        HealValue = healValue;
        CanRevive = canRevive;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (pawn.Health == 0)
            return;

        pawn.Health = Mathf.Clamp(pawn.Health + HealValue, 0, pawn.MaxHealth);
        pawnController.ReeceiveHeal(CanRevive);
    }
}