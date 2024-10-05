using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    private PawnController AbilityUser { get; set; }
    public string Animation { get; set; }
    public float Delay { get; set; }
    public float Range { get; set; }
    public Vector3 Destination => FocusComponent.Destination;

    private AbilityEffect Effect { get; set; }
    private AbilityFocusComponent FocusComponent { get; set; }
    private AbilityResourceComponent ResourceComponent { get; set; }
    private AbilityActionComponent ActionComponent { get; set; }
    
    public Ability(
        PawnController abilityUser,
        string animation, 
        Damage damage,
        float range, 
        float delay, 
        TargetType target, 
        FocusType focus,
        int error,
        int manaCost,
        ProjectileController projectile)
    {
        AbilityUser = abilityUser;
        Animation = animation;
        Delay = delay;
        Range = range;

        if (damage.Value > 0)
        {
            Effect = new DamageEffect(damage);
        }
        else
        {
            Effect = new HealEffect(-damage.Value, false);
        }
        
        FocusComponent = new AbilityFocusComponent(AbilityUser, target, focus, error);
        ResourceComponent = new AbilityResourceComponent(AbilityUser, manaCost);
        
        ActionComponent = projectile != null ? 
            new ProjectileActionComponent(AbilityUser, FocusComponent, Effect, projectile) : 
            new InstantActionComponent(AbilityUser, FocusComponent, Effect);
    }
    
    public void ChooseFocus(List<PawnController> pawns)
    {
        FocusComponent.ChooseFocus(pawns);
    }
    
    public bool HasResource()
    {
        return ResourceComponent.HasResource();
    }
    
    public void SpendResource()
    {
        ResourceComponent.SpendResource();
    }
    
    public void DoAction()
    {
        ActionComponent.DoAction();
    }
}