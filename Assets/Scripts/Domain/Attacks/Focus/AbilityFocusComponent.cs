using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityFocusComponent
{
    private PawnController AbilityUser { get; set; }
    private TargetType Target { get; set; }
    private FocusType Focus { get; set; }
    private int Error { get; set; }
    
    public Vector3 Destination => HasFocus ? FocusedPawn.transform.position : AbilityUser.transform.position;
    private bool HasFocus { get; set; }
    internal PawnController FocusedPawn { get; set; }

    public AbilityFocusComponent(PawnController abilityUser, TargetType target, FocusType focus, int error)
    {
        AbilityUser = abilityUser;
        Target = target;
        Focus = focus;
        Error = error;
        HasFocus = false;
        FocusedPawn = null;
    }

    public void ChooseFocus(List<PawnController> pawns)
    {
        bool WherePredicate(PawnController pawn)
        {
            return Target switch
            {
                TargetType.Enemy => pawn.Team != AbilityUser.Team && pawn.PawnState.CanBeTargeted,
                TargetType.Ally => pawn.Team == AbilityUser.Team && pawn.PawnState.CanBeTargeted && pawn.PawnState.AbleToFight,
                _ => throw new ArgumentOutOfRangeException()
            };
        };

        float OrderPredicate(PawnController pawn)
        {
            return Focus switch
            {
                FocusType.Self => pawn == AbilityUser ? 0 : 1,
                FocusType.Closest => pawn == AbilityUser ? 1000 : (pawn.transform.position - AbilityUser.transform.position).sqrMagnitude,
                FocusType.Farthest => pawn == AbilityUser ? 1000 : 1000 - (pawn.transform.position - AbilityUser.transform.position).sqrMagnitude,
                FocusType.LowestLife => pawn == AbilityUser ? 1000 : pawn.Pawn.Health,
                FocusType.HighestLife => pawn == AbilityUser ? 1000 : 1000 - pawn.Pawn.Health,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        var selectedPawns = pawns
            .Where(WherePredicate)
            .OrderBy(OrderPredicate)
            .Take(1 + Error)
            .ToList();

        if (selectedPawns.Count == 0)
            return;
        
        HasFocus = true;
        FocusedPawn = selectedPawns[Random.Range(0, selectedPawns.Count)];
    }
}