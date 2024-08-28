using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ArenaController : MonoBehaviour
{
    public List<PawnController> ActivePawns { get; private set; }
    private List<PawnController> EnemyPawns { get; set; }
    private List<PawnController> InitiativeList { get; set; }
    private PlayerController Player { get; set; }
    private List<EnemyController> Enemies { get; set; }

    public void Init(PlayerController player, List<EnemyController> enemies)
    {
        Player = player;
        Enemies = enemies;
    }

    private void Start()
    {
        ActivePawns = new List<PawnController>();
        EnemyPawns = new List<PawnController>();
        InitiativeList = new List<PawnController>();

        SpawnPlayerPawn();
        
        foreach (var enemyController in Enemies)
        {
            var pawn = enemyController.PawnData;
            var pawnController = enemyController.GetComponent<PawnController>();

            pawnController.enabled = true;
            enemyController.GetComponent<NavMeshAgent>().enabled = true;
            
            EnemyPawns.Add(pawnController);
            pawnController.Init(this, pawn.ToDomain());
        }
    }

    private void SpawnPlayerPawn()
    {
        Player.enabled = false;
        
        var playerMovementController = Player.GetComponent<PlayerMovementController>();
        playerMovementController.enabled = false;
        playerMovementController.Prepare();
        
        var pawn = Player.PawnData;
        var pawnController = Player.GetComponent<PawnController>();
        
        pawnController.enabled = true;
        Player.GetComponent<NavMeshAgent>().enabled = true;
        
        ActivePawns.Add(pawnController);
        pawnController.Init(this, pawn.ToDomain());
    }

    public void PlayBattle()
    {
        var pawnsList = new List<PawnController>();
        pawnsList.AddRange(ActivePawns);
        pawnsList.AddRange(EnemyPawns);

        InitiativeList = pawnsList.OrderBy(p => p.Pawn.Initiative).ToList();

        StartCoroutine(BattleCoroutine());
    }

    private IEnumerator BattleCoroutine()
    {
        var hasEnemies = true;
        var hasPlayers = true;

        while (hasEnemies && hasPlayers)
        {
            yield return RealizaTurno();

            hasEnemies = EnemyPawns.Count(e => e.PawnState.AbleToFight) > 0;
            hasPlayers = ActivePawns.Count(e => e.PawnState.AbleToFight) > 0;
        }

        if (hasEnemies)
        {
            Debug.Log("enemies win");
        }

        if (hasPlayers)
        {
            Debug.Log("player win");
            yield return new WaitForSeconds(3f);
            EndBattle();
        }
    }

    private IEnumerator RealizaTurno()
    {
        foreach (var pawn in InitiativeList)
        {
            if (!pawn.PawnState.CanTakeTurn) continue;
            yield return pawn.Turno(InitiativeList);
        }
    }

    private void EndBattle()
    {
        foreach (var enemyPawn in EnemyPawns)
        {
            enemyPawn.Deactivate();
        }
        
        Application.Instance.SceneManager.EndBattleScene();
    }
}