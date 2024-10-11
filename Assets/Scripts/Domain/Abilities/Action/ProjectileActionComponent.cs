using System.Collections.Generic;

public class ProjectileActionComponent : AbilityActionComponent
{
    private ProjectileController Projectile { get; set; }

    public ProjectileActionComponent(
        PawnController abilityUser,
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent,
        ProjectileController projectile) : base(abilityUser, effects, abilityFocusComponent)
    {
        Projectile = projectile;
    }

    public override void DoAction()
    {
        AbilityUser.SpawnProjectile(Projectile, Effects, AbilityFocusComponent.FocusedPawn);
    }
}