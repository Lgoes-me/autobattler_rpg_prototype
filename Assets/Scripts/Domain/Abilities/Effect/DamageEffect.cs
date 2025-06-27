public class DamageEffect : AbilityEffect
{
    private DamageDomain Damage { get; set; }

    public DamageEffect(PawnController abilityUser, DamageDomain damage) : base(abilityUser)
    {
        Damage = damage;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var resources = focus.Pawn.GetComponent<ResourceComponent>();

        if (!resources.IsAlive)
            return;

        resources.ReceiveDamage(Damage);
    }
}