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

        if (pawn.Stats.Health == 0)
            return;

        pawn.Stats.Health = Mathf.Clamp(pawn.Stats.Health + HealValue, 0, pawn.Stats.MaxHealth);
        pawnController.ReeceiveHeal(CanRevive);
    }
}