public class DamageEffect : AbilityEffect
{
    private DamageDomain Damage { get; set; }

    public DamageEffect(PawnController abilityUser, DamageDomain damage) : base(abilityUser)
    {
        Damage = damage;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var stats = focus.Pawn.GetComponent<StatsComponent>();

        if (!stats.IsAlive)
            return;

        stats.ReceiveDamage(Damage);
    }
}