using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    private PawnController AbilityUser { get; set; }
    public string Animation { get; set; }
    public float Delay { get; set; }

    private AbilityEffect Effect { get; set; }
    public AbilityFocusComponent Focus { get; private set; }
    private AbilityResourceComponent Resource { get; set; }
    private AbilityActionComponent Action { get; set; }
    

    public Ability(
        PawnController abilityUser,
        string animation,
        float delay,
        AbilityEffect effect,
        AbilityFocusComponent focus,
        AbilityResourceComponent resource,
        AbilityActionComponent action)
    {
        AbilityUser = abilityUser;
        Animation = animation;
        Delay = delay;

        Effect = effect;
        Focus = focus;
        Resource = resource;
        Action = action;
    }
    
    public void ChooseFocus(Battle battle)
    {
        Focus.ChooseFocus(battle);
    }
    
    public bool CanUse()
    {
        return Resource.HasResource();
    }
    
    public void SpendResource()
    {
        Resource.SpendResource();
    }
    
    public void DoAction()
    {
        Action.DoAction();
    }
}