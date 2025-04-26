using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaOfEffectActionComponent : AbilityActionComponent
{
    private float Range { get; set; }
    
    public AreaOfEffectActionComponent(
        PawnController abilityUser, 
        List<AbilityEffect> effects,
        AbilityFocusComponent abilityFocusComponent,
        float range) : base(abilityUser, effects, abilityFocusComponent)
    {
        Range = range;
    }

    public override void DoAction()
    {
        var closePawns = AbilityUser.Battle.Pawns
            .Where(p => 
                p.Pawn.Team != AbilityUser.Pawn.Team && 
                Vector3.Distance(AbilityUser.transform.position, p.transform.position) < Range)
            .ToList();

        foreach (var pawn in closePawns)
        {
            foreach (var effect in Effects)
            {
                effect.DoAbilityEffect(pawn);
            }
        }
    }
}