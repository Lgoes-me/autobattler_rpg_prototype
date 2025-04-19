public class DamageEffect : AbilityEffect
{
    private DamageDomain Damage { get; set; }

    public DamageEffect(PawnController abilityUser, DamageDomain damage) : base(abilityUser)
    {
        Damage = damage;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        if (!focus.Pawn.IsAlive)
            return;

        focus.Pawn.ReceiveDamage(Damage);
        focus.ReceiveAttack();
    }
}