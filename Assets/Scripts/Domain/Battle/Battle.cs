﻿using System;
using System.Collections.Generic;
using System.Linq;

public class Battle
{
    public string Id { get; }

    public List<PawnController> EnemyPawns { get; }
    public List<PawnController> PlayerPawns { get; }
    public List<PawnController> Pawns { get; }

    public bool HasEnemies => EnemyPawns.Count(e => e.PawnState.AbleToFight) > 0;
    public bool HasPlayers => PlayerPawns.Count(e => e.PawnState.AbleToFight) > 0;

    public Battle(string id, List<PawnController> enemyPawns, List<PawnController> playerPawns)
    {
        Id = id;

        EnemyPawns = enemyPawns;
        PlayerPawns = playerPawns;

        Pawns = new List<PawnController>();
        Pawns.AddRange(EnemyPawns);
        Pawns.AddRange(PlayerPawns);
    }

    public PawnController QueryEnemies(PawnController user, FocusType focusType, int error)
    {
        var pawns = user.Pawn.Team == TeamType.Player ? EnemyPawns : PlayerPawns;

        pawns.OrderBy(OrderPredicate).Take(1 + error).ToList();

        float OrderPredicate(PawnController pawn)
        {
            return focusType switch
            {
                FocusType.Closest => (pawn.transform.position - user.transform.position).sqrMagnitude,
                FocusType.Farthest => 1000 - (pawn.transform.position - user.transform.position).sqrMagnitude,
                FocusType.LowestLife => pawn.Pawn.Health,
                FocusType.HighestLife => 1000 - pawn.Pawn.Health,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        if (pawns.Count == 0)
            return null;

        return pawns[UnityEngine.Random.Range(0, pawns.Count)];
    }

    public PawnController QueryAlly(PawnController user, FocusType focusType, int error)
    {
        float OrderPredicate(PawnController pawn)
        {
            return focusType switch
            {
                FocusType.Unknown => 1,
                FocusType.Closest => pawn == user
                    ? 1000
                    : (pawn.transform.position - user.transform.position).sqrMagnitude,
                FocusType.Farthest => pawn == user
                    ? 1000
                    : 1000 - (pawn.transform.position - user.transform.position).sqrMagnitude,
                FocusType.LowestLife => pawn == user ? 1000 : pawn.Pawn.Health,
                FocusType.HighestLife => pawn == user ? 1000 : 1000 - pawn.Pawn.Health,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        var selectedPawns = Pawns.OrderBy(OrderPredicate).Take(1 + error).ToList();

        if (selectedPawns.Count == 0)
            return null;

        return selectedPawns[UnityEngine.Random.Range(0, selectedPawns.Count)];
    }

    public PawnController Query(PawnController user, TargetType targetType, FocusType focusType, int error)
    {
        bool WherePredicate(PawnController pawn)
        {
            return targetType switch
            {
                TargetType.Self => pawn == user,
                TargetType.Enemy => pawn.Pawn.Team != user.Pawn.Team && pawn.PawnState.CanBeTargeted,
                TargetType.Ally => pawn.Pawn.Team == user.Pawn.Team && pawn.PawnState.CanBeTargeted &&
                                   pawn.PawnState.AbleToFight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        float OrderPredicate(PawnController pawn)
        {
            return focusType switch
            {
                FocusType.Unknown => 1,
                FocusType.Closest => pawn == user
                    ? 1000
                    : (pawn.transform.position - user.transform.position).sqrMagnitude,
                FocusType.Farthest => pawn == user
                    ? 1000
                    : 1000 - (pawn.transform.position - user.transform.position).sqrMagnitude,
                FocusType.LowestLife => pawn == user ? 1000 : pawn.Pawn.Health,
                FocusType.HighestLife => pawn == user ? 1000 : 1000 - pawn.Pawn.Health,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        var selectedPawns = Pawns.Where(WherePredicate).OrderBy(OrderPredicate).Take(1 + error).ToList();

        if (selectedPawns.Count == 0)
            return null;

        return selectedPawns[UnityEngine.Random.Range(0, selectedPawns.Count)];
    }

    
    
    
    public List<PawnController> GetInitiativeList()
    {
        return Pawns.OrderByDescending(p => p.Pawn.Initiative).ToList();
    }
}