public class DamageOverTimeEffect : AbilityEffect
{
    private DamageOverTimeBuff DamageOverTimeBuff { get; set; }

    public DamageOverTimeEffect(PawnController abilityUser, DamageOverTimeBuff damageOverTimeBuff) : base(abilityUser)
    {
        DamageOverTimeBuff = damageOverTimeBuff;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (pawn.Stats.Health == 0)
            return;

        DamageOverTimeBuff.OnDamageDealt = pawnController.ReceiveAttack;
        pawn.AddBuff(DamageOverTimeBuff);
        pawnController.ReceiveDebuff();
    }
}
