using System.Collections.Generic;
using UnityEngine;

public class ProjectileActionComponent : AbilityActionComponent
{
    private ProjectileController Projectile { get; set; }
    private AnimationCurve Trajectory { get; set; }
    private bool OverrideProjectile { get; set; }
    private List<AbilityBehaviour> ExtraEffects { get; set; }

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
        ExtraEffects = new List<AbilityBehaviour>();
    }

    public void AddExtraEffects(AbilityBehaviour abilityBehaviour)
    {
        ExtraEffects.Add(abilityBehaviour);
    }

    public override void DoAction()
    {
        AbilityUser.SpawnProjectile(
            Projectile,
            Trajectory,
            Effects,
            AbilityFocusComponent.FocusedPawn,
            OverrideProjectile,
            ExtraEffects);
    }
}