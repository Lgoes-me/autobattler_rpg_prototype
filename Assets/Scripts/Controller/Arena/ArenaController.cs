using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    private List<PawnController> ActivePawns { get; set; }
    private List<PawnController> EnemyPawns { get; set; }
    private List<PawnController> InitiativeList { get; set; }
    
    private SceneManager SceneManager { get; set; }
    private List<EnemyController> Enemies { get; set; }

    public void Init(SceneManager sceneManager, List<EnemyController> enemies)
    {
        SceneManager = sceneManager;
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
            EnemyPawns.Add(enemyController.GetPawnController());
        }
    }

    private void SpawnPlayerPawn()
    {
        ActivePawns.Add(Application.Instance.PlayerManager.GetPawnController());
    }

    public void AddPlayerPawn(PawnController pawn)
    {
        ActivePawns.Add(pawn);
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
            foreach (var pawn in InitiativeList)
            {
                if (!pawn.PawnState.CanTakeTurn) continue;
                yield return pawn.Turno(InitiativeList);
            }

            hasEnemies = EnemyPawns.Any(e => e.PawnState.AbleToFight);
            hasPlayers = ActivePawns.Any(e => e.PawnState.AbleToFight);
        }

        if (hasEnemies)
        {
            Debug.Log("enemies win");
            //Death
        }

        if (hasPlayers)
        {
            Debug.Log("player win");
            
            foreach (var activePawn in ActivePawns)
            {
                if (!activePawn.PawnState.AbleToFight) continue;
                //activePawn.Dance();
            }
            
            yield return new WaitForSeconds(3f);

            foreach (var enemyPawn in EnemyPawns)
            {
                enemyPawn.Deactivate();
            }

            SceneManager.EndBattleScene();
        }
    }
}