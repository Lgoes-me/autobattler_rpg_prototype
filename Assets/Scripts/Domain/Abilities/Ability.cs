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
        float range,
        float delay,
        AbilityEffect effect,
        AbilityFocusComponent focusComponent,
        AbilityResourceComponent resourceComponent,
        AbilityActionComponent actionComponent)
    {
        AbilityUser = abilityUser;
        Animation = animation;
        Range = range;
        Delay = delay;

        Effect = effect;
        FocusComponent = focusComponent;
        ResourceComponent = resourceComponent;
        ActionComponent = actionComponent;
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