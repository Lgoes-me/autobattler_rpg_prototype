using System.Collections.Generic;
using UnityEngine;

public class ProjectileActionComponent : AbilityActionComponent
{
    private ProjectileController Projectile { get; set; }
    private AnimationCurve Trajectory { get; set; }
    private bool OverrideProjectile { get; set; }

    public ProjectileActionComponent(
        PawnController abilityUser,
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent,
        ProjectileController projectile,
        AnimationCurve trajectory,
        bool overrideProjectile) : base(abilityUser, effects, abilityFocusComponent)
    {
        Projectile = projectile;
        Trajectory = trajectory;
        OverrideProjectile = overrideProjectile;
    }

    public override void DoAction()
    {
        AbilityUser.SpawnProjectile(
            Projectile,
            Trajectory,
            Effects,
            AbilityFocusComponent.FocusedPawn,
            OverrideProjectile);
    }
}