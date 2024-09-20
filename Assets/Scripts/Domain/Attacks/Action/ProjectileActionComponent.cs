public class ProjectileActionComponent : AbilityActionComponent
{
    private ProjectileController Projectile { get; set; }

    public ProjectileActionComponent(
        PawnController abilityUser, 
        AbilityFocusComponent focusComponent,
        AbilityEffect effect,
        ProjectileController projectile) : base(abilityUser, focusComponent, effect)
    {
        Projectile = projectile;
    }

    public override void DoAction()
    {
        AbilityUser.SpawnProjectile(Projectile, Effect);
    }
}