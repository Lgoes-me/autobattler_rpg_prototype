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

        if (pawn.Stats.Health == 0)
            return;

        pawn.Stats.ReceiveDamage(Damage);
        pawnController.ReceiveAttack();
    }
}

public enum DamageType
{
    Slash = 1,
    Magical = 2,
}