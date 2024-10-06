using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LowHealthModifier : PriorityModifier
{
    [field: SerializeField] private TargetType Target { get; set; }
    [field: SerializeField] private FocusType Focus { get; set; }
    [field: SerializeField] private int Health { get; set; }
    
    public override int AlterPriority(PawnController abilityUser, List<PawnController> pawns, int priority)
    {
        bool WherePredicate(PawnController pawn)
        {
            return Target switch
            {
                TargetType.Self => pawn == abilityUser,
                TargetType.Enemy => pawn.Team != abilityUser.Team && pawn.PawnState.CanBeTargeted,
                TargetType.Ally => pawn.Team == abilityUser.Team && pawn.PawnState.CanBeTargeted &&
                                   pawn.PawnState.AbleToFight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        float OrderPredicate(PawnController pawn)
        {
            return Focus switch
            {
                FocusType.Unknown => 1,
                FocusType.Closest => pawn == abilityUser
                    ? 1000
                    : (pawn.transform.position - abilityUser.transform.position).sqrMagnitude,
                FocusType.Farthest => pawn == abilityUser
                    ? 1000
                    : 1000 - (pawn.transform.position - abilityUser.transform.position).sqrMagnitude,
                FocusType.LowestLife => pawn == abilityUser ? 1000 : pawn.Pawn.Health,
                FocusType.HighestLife => pawn == abilityUser ? 1000 : 1000 - pawn.Pawn.Health,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        var selectedPawn = pawns
            .Where(WherePredicate)
            .OrderBy(OrderPredicate)
            .FirstOrDefault();
        
        if (selectedPawn != null && selectedPawn.Pawn.Health < Health)
            return priority + 1;
        
        return priority;
    }
}