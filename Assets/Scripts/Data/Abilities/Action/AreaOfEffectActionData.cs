using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AreaOfEffectActionData : ActionData
{
    [field: SerializeField] private float Range { get; set; }
    
    public override AbilityActionComponent ToDomain(
        PawnController abilityUser,
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent)
    {
        return new AreaOfEffectActionComponent(abilityUser, effects, abilityFocusComponent, Range);
    }
}