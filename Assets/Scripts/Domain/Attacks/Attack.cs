using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack
{
    private PawnController PawnController { get; set; }
    public string Animation { get; set; }
    public Damage Damage { get; set; }
    public int Range { get; set; }
    public float Delay { get; set; }
    private TargetType Target { get; set; }
    private FocusType Focus { get; set; }
    private int Error { get; set; }
    public float ManaCost { get; set; }
    
    public Vector3 Destination => HasFocus ? FocusedPawn.transform.position : PawnController.transform.position;
    private bool HasFocus { get; set; }
    private PawnController FocusedPawn { get; set; }

    public Attack(
        PawnController pawnController,
        string animation, 
        Damage damage,
        int range, 
        float delay, 
        TargetType target, 
        FocusType focus,
        int error,
        float manaCost)
    {
        PawnController = pawnController;
        Animation = animation;
        Damage = damage;
        Range = range;
        Delay = delay;
        Target = target;
        Focus = focus;
        Error = error;
        ManaCost = manaCost;
    }

    public void ChooseFocus(List<PawnController> pawns)
    {
        bool WherePredicate(PawnController pawn)
        {
            return Target switch
            {
                TargetType.Enemy => pawn.Team != PawnController.Team && pawn.PawnState.CanBeTargeted,
                TargetType.Ally => pawn.Team == PawnController.Team && pawn.PawnState.AbleToFight,
                _ => throw new ArgumentOutOfRangeException()
            };
        };

        float OrderPredicate(PawnController pawn)
        {
            return Focus switch
            {
                FocusType.Self => pawn == PawnController ? 0 : 1,
                FocusType.Closest => (pawn.transform.position - PawnController.transform.position).sqrMagnitude,
                FocusType.Farthest => 100 - (pawn.transform.position - PawnController.transform.position).sqrMagnitude,
                FocusType.LowestLife => pawn.Pawn.Health,
                FocusType.HighestLife => 100 - pawn.Pawn.Health,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        var selectedPawns = pawns
                .Where(WherePredicate)
                .OrderBy(OrderPredicate)
                .Take(Error)
                .ToList();

        if (selectedPawns.Count == 0)
            return;
        
        HasFocus = true;
        FocusedPawn = selectedPawns[Random.Range(0, selectedPawns.Count)];
    }

    public void DoAttack()
    {
        FocusedPawn.ReceiveAttack(this);
    }
}