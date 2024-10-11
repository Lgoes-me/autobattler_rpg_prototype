public class DamageEffect : AbilityEffect
{
    private DamageDomain Damage { get; set; }

    public DamageEffect(PawnController abilityUser, DamageDomain damage) : base(abilityUser)
    {
        Damage = damage;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;

        if (!pawn.IsAlive)
            return;

        pawn.ReceiveDamage(Damage);
        pawnController.ReceiveAttack();
    }
}