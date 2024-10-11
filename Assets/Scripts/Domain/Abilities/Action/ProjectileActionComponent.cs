public class ProjectileActionComponent : AbilityActionComponent
{
    private ProjectileController Projectile { get; set; }

    public ProjectileActionComponent(PawnController abilityUser, AbilityEffect effect, ProjectileController projectile)
        : base(abilityUser, effect)
    {
        Projectile = projectile;
    }

    public override void DoAction()
    {
        AbilityUser.SpawnProjectile(Projectile, Effect);
    }
}