using System;
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
        return Query(pawns, user, focusType, error);
    }

    public PawnController QueryAlly(PawnController user, FocusType focusType, bool canTargetSelf)
    {
        var pawns = user.Pawn.Team == TeamType.Player ? EnemyPawns : PlayerPawns;
        
        if (!canTargetSelf)
        {
            pawns.Remove(user);
        }

        return Query(pawns, user, focusType, 0);
    }
    
    private PawnController Query(List<PawnController> pawns, PawnController user, FocusType focusType, int error)
    {
        var selectedPawns = 
            pawns
                .Where(p => p.PawnState.CanBeTargeted)
                .OrderBy(OrderPredicate)
                .Take(1 + error)
                .ToList();

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
        
        if (selectedPawns.Count == 0)
            return null;

        return selectedPawns[UnityEngine.Random.Range(0, selectedPawns.Count)];
    }
    public List<PawnController> GetInitiativeList()
    {
        return Pawns.OrderByDescending(p => p.Pawn.Initiative).ToList();
    }
}