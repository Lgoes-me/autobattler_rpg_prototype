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
        FocusedPawn.transform.position + 
        Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * (Range - 1);

    private List<PawnController> FocusedPawns { get; set; }
    private PawnController FocusedPawn => FocusedPawns[0];
    private bool IsInRange => Range >= (FocusedPawn.transform.position - AbilityUser.transform.position).magnitude;
    public bool IsSpecial { get; }

    public Ability(
        PawnController abilityUser,
        string animation,
        float delay,
        float range,
        List<AbilityBehaviour> abilityBehaviours,
        AbilityResourceComponent resource,
        bool isSpecial)
    {
        AbilityUser = abilityUser;
        Animation = animation;
        Delay = delay;
        Range = range;
        
        AbilityBehaviours = abilityBehaviours;
        Resource = resource;
        IsSpecial = isSpecial;
        
        FocusedPawns = new List<PawnController>();
    }

    public void ChooseFocus(Battle battle)
    {
        foreach (var abilityBehaviour in AbilityBehaviours)
        {
            FocusedPawns.Add(abilityBehaviour.ChooseFocus(battle));
        }
    }

    public bool ShouldUse()
    {
        return IsInRange;
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
        foreach (var abilityBehaviour in AbilityBehaviours)
        {
            abilityBehaviour.DoAction();
        }
    }
}