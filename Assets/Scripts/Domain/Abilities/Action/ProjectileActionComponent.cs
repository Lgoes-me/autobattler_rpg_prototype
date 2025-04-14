using System.Collections.Generic;
using UnityEngine;

public class ProjectileActionComponent : AbilityActionComponent
{
    private ProjectileController Projectile { get; set; }
    private AnimationCurve Trajectory { get; set; }

    public ProjectileActionComponent(PawnController abilityUser,
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent,
        ProjectileController projectile, 
        AnimationCurve trajectory) : base(abilityUser, effects, abilityFocusComponent)
    {
        Projectile = projectile;
        Trajectory = trajectory;
    }

    public override void DoAction()
    {
        AbilityUser.SpawnProjectile(Projectile, Trajectory, Effects, AbilityFocusComponent.FocusedPawn);
    }
}