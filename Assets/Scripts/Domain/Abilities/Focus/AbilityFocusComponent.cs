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
    private float Range { get; set; }
    private int Error { get; set; }

    public bool IsInRange => Range >= (FocusedPawnPosition - AbilityUser.transform.position).magnitude;
    public Vector3 FocusedPawnPosition => FocusedPawn != null ? FocusedPawn.transform.position : AbilityUser.transform.position;
    public Vector3 WalkingDestination =>
        FocusedPawn != null ? 
            FocusedPawn.transform.position + Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * (Range - 1)
            : AbilityUser.transform.position;
    
    public PawnController FocusedPawn { get; private set; }

    public AbilityFocusComponent(PawnController abilityUser, TargetType target, FocusType focus, float range, int error)
    {
        AbilityUser = abilityUser;
        Target = target;
        Focus = focus;
        Range = range;
        Error = error;
        FocusedPawn = null;
    }

    public void ChooseFocus(List<PawnController> pawns)
    {
        bool WherePredicate(PawnController pawn)
        {
            return Target switch
            {
                TargetType.Enemy => pawn.Team != AbilityUser.Team && pawn.PawnState.CanBeTargeted,
                TargetType.Ally => pawn.Team == AbilityUser.Team && pawn.PawnState.CanBeTargeted &&
                                   pawn.PawnState.AbleToFight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        ;

        float OrderPredicate(PawnController pawn)
        {
            return Focus switch
            {
                FocusType.Self => pawn == AbilityUser ? 0 : 1,
                FocusType.Closest => pawn == AbilityUser
                    ? 1000
                    : (pawn.transform.position - AbilityUser.transform.position).sqrMagnitude,
                FocusType.Farthest => pawn == AbilityUser
                    ? 1000
                    : 1000 - (pawn.transform.position - AbilityUser.transform.position).sqrMagnitude,
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

        FocusedPawn = selectedPawns[Random.Range(0, selectedPawns.Count)];
    }
}

public enum TargetType
{
    Unknown = 0,
    Ally = 1,
    Enemy = 2
}

public enum FocusType
{
    Self = 0,
    Closest = 1,
    Farthest = 2,
    LowestLife = 3,
    HighestLife = 4
}