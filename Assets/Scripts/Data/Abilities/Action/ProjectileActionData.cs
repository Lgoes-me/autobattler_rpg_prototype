﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileActionData : ActionData
{
    [field: SerializeField] private ProjectileController Projectile { get; set; }
    [field: SerializeField] private AnimationCurve Trajectory { get; set; }
    [field: SerializeField] private bool OverrideProjectileSprite { get; set; }
    
    public override AbilityActionComponent ToDomain(
        PawnController abilityUser, 
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent)
    {
        return new ProjectileActionComponent(
            abilityUser, 
            effects, 
            abilityFocusComponent, 
            Projectile, 
            Trajectory, 
            OverrideProjectileSprite);
    }
}