using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    private PawnController AbilityUser { get; set; }
    public string Animation { get; set; }
    public float Delay { get; set; }
    private float Range { get; set; }

    private List<AbilityBehaviour> AbilityBehaviours { get; set; }
    private AbilityResourceComponent Resource { get; set; }
    
    public Vector3 WalkingDestination => 
        AbilityUser.Pawn.Focus.transform.position + 
        Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * (Range - 1);

    private bool IsInRange => Range >= (AbilityUser.Pawn.Focus.transform.position - AbilityUser.transform.position).magnitude;
    
    public bool IsSpecial => Resource is not NoResourceComponent;
    public bool Used { get; set; }

    public Ability(
        PawnController abilityUser,
        string animation,
        float delay,
        float range,
        List<AbilityBehaviour> abilityBehaviours,
        AbilityResourceComponent resource)
    {
        AbilityUser = abilityUser;
        Animation = animation;
        Delay = delay;
        Range = range;
        
        AbilityBehaviours = abilityBehaviours;
        Resource = resource;
    }
    
    public void ChooseFocus(PawnController pawnController, Battle battle)
    {
        foreach (var abilityBehaviour in AbilityBehaviours)
        {
            abilityBehaviour.ChooseFocus(pawnController, battle);
        }
    }
    
    public bool ShouldUse()
    {
        return IsInRange && !Used;
    }

    public void SpendResource()
    {
        Resource.SpendResource();
    }
    
    public void AddEffect(AbilityBehaviour effect, EffectType weaponEffectType)
    {
        switch (weaponEffectType)
        {
            case EffectType.InstantAction:
            {
                AbilityBehaviours.Add(effect);
                break;
            }
            
            case EffectType.ProjectileHit:
            {
                foreach (var abilityBehaviour in AbilityBehaviours)
                {
                    if(abilityBehaviour.Action is not ProjectileActionComponent projectileActionComponent)
                        continue;

                    projectileActionComponent.AddExtraEffects(effect);
                }

                break;
            }
        }
    }
    
    public void DoAction()
    {
        foreach (var abilityBehaviour in AbilityBehaviours)
        {
            abilityBehaviour.DoAction();
        }
    }
}