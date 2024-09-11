using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    private List<PawnController> ActivePawns { get; set; }
    private List<PawnController> EnemyPawns { get; set; }
    private List<PawnController> InitiativeList { get; set; }

    private string BattleId { get; set; }
    private SceneManager SceneManager { get; set; }
    private List<EnemyController> Enemies { get; set; }

    public void Init(string battleId, SceneManager sceneManager, List<EnemyController> enemies)
    {
        BattleId = battleId;
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
            var pawnController = enemyController.GetPawnController();
            EnemyPawns.Add(pawnController);
        }
    }

    private void SpawnPlayerPawn()
    {
        var pawnController = Application.Instance.PlayerManager.GetPawnController();
        ActivePawns.Add(pawnController);
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
            Application.Instance.AudioManager.PlayMusic(MusicType.Victory);
            
            foreach (var playerPawn in ActivePawns)
            {
                if (!playerPawn.PawnState.AbleToFight) continue;
                playerPawn.Dance();
            }

            yield return new WaitForSeconds(5f);
            
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

        Application.Instance.PlayerManager.AddDefeated(BattleId);
        SceneManager.EndBattleScene();
    }
}