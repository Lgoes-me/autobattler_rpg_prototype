using System.Collections.Generic;
using UnityEngine;

public class ProjectileActionComponent : AbilityActionComponent
{
    private ProjectileController Projectile { get; set; }
    private AnimationCurve Trajectory { get; set; }
    private int Error { get; set; }

    public ProjectileActionComponent(PawnController abilityUser,
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent,
        ProjectileController projectile, 
        AnimationCurve trajectory,
        int error) : base(abilityUser, effects, abilityFocusComponent)
    {
        Projectile = projectile;
        Trajectory = trajectory;
        Error = error;
    }

    public override void DoAction()
    {
        AbilityUser.SpawnProjectile(Projectile, Trajectory, Effects, AbilityFocusComponent.FocusedPawn, Error);
    }
}