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
    private int Quantidade { get; set; }
    private int Error { get; set; }
    
    public Vector3 Destination => HasFocus ? FocusedPawns[0].transform.position : AbilityUser.transform.position;
    public List<PawnController> FocusedPawns { get; set; }
    
    private bool HasFocus { get; set; }

    public AbilityFocusComponent(PawnController abilityUser, TargetType target, FocusType focus, int error)
    {
        AbilityUser = abilityUser;
        Target = target;
        Focus = focus;
        Error = error;
        HasFocus = false;
        Quantidade = 1;
        FocusedPawns = new List<PawnController>();
    }

    public void ChooseFocus(List<PawnController> pawns)
    {
        FocusedPawns.Clear();
        
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

        for (int i = 0; i < Mathf.Min(Quantidade, selectedPawns.Count); i++)
        {
            var randomPawn = selectedPawns[Random.Range(0, selectedPawns.Count)];
            selectedPawns.Remove(randomPawn);
            
            FocusedPawns.Add(randomPawn);
        }
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