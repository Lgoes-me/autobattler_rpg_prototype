﻿using System;
using System.Collections.Generic;
using System.Linq;

public class Battle
{
    public string Id { get; }

    public List<PawnController> EnemyPawns { get; }
    public List<PawnController> PlayerPawns { get; }
    public List<PawnController> Pawns { get; }

    public bool HasEnemies => EnemyPawns.Count(e => e.PawnState.AbleToFight || e.PawnState.WillRevive) > 0;
    public bool HasPlayers => PlayerPawns.Count(e => e.PawnState.AbleToFight || e.PawnState.WillRevive) > 0;

    public Battle(string id)
    {
        Id = id;

        EnemyPawns = new List<PawnController>();
        PlayerPawns = new List<PawnController>();
        Pawns = new List<PawnController>();
    }

    public void AddEnemy(PawnController enemyPawn)
    {
        EnemyPawns.Add(enemyPawn);
        Pawns.Add(enemyPawn);
    }

    public void AddPlayerPawn(PawnController playerPawn)
    {
        PlayerPawns.Add(playerPawn);
        Pawns.Add(playerPawn);
    }

    public PawnController QueryEnemies(PawnController user, FocusType focusType)
    {
        var pawns = user.Pawn.Team == TeamType.Player ? EnemyPawns : PlayerPawns;
        return Query(pawns, user, focusType);
    }

    public PawnController QueryAlly(PawnController user, FocusType focusType, bool canTargetSelf)
    {
        var pawns = user.Pawn.Team == TeamType.Player ? EnemyPawns : PlayerPawns;

        if (!canTargetSelf)
        {
            pawns.Remove(user);
        }

        return Query(pawns, user, focusType);
    }

    private PawnController Query(List<PawnController> pawns, PawnController user, FocusType focusType)
    {
        var selectedPawns =
            pawns
                .Where(p => p.PawnState.CanBeTargeted)
                .OrderBy(OrderPredicate)
                .Take(1)
                .ToList();

        float OrderPredicate(PawnController pawn)
        {
            return focusType switch
            {
                FocusType.Closest => (pawn.transform.position - user.transform.position).sqrMagnitude,
                FocusType.Farthest => 1000 - (pawn.transform.position - user.transform.position).sqrMagnitude,
                FocusType.LowestLife => pawn.Pawn.GetComponent<ResourceComponent>().Health,
                FocusType.HighestLife => 1000 - pawn.Pawn.GetComponent<ResourceComponent>().Health,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        if (selectedPawns.Count == 0)
            return null;

        return selectedPawns[UnityEngine.Random.Range(0, selectedPawns.Count)];
    }
}