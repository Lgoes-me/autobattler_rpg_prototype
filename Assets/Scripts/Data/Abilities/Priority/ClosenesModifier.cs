using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CloseEnemyModifier : PriorityModifier
{
    [field: SerializeField] private float Distance { get; set; }

    public override int AlterPriority(PawnController abilityUser, List<PawnController> pawns, BaseFocusData focusData,
        int priority)
    {
        bool WherePredicate(PawnController pawn)
        {
            return focusData.Target switch
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
            return focusData.Focus switch
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

        if (selectedPawn != null && (abilityUser.transform.position - selectedPawn.transform.position).magnitude < Distance)
            return priority + 1 * Multiplier;

        return priority;
    }
}