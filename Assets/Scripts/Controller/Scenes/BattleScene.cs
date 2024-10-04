using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleScene : BaseScene
{
    [field: SerializeField] private GameObject BattleCanvas { get; set; }
    [field: SerializeField] private GameObject BattleLostCanvas { get; set; }
    [field: SerializeField] private List<PawnCanvasController> PawnCanvases { get; set; }
    [field: SerializeField] private BossCanvasController BossCanvas { get; set; }
    
    private List<PawnController> ActivePawns { get; set; }
    private List<PawnController> EnemyPawns { get; set; }
    private List<PawnController> InitiativeList { get; set; }

    private string BattleId { get; set; }
    private List<EnemyInfo> Enemies { get; set; }

    public void ActivateBattleScene(string battleId, List<EnemyInfo> enemies)
    {
        BattleId = battleId;
        Enemies = enemies;
    }
    
    private void Start()
    {
        ActivePawns = new List<PawnController>();
        EnemyPawns = new List<PawnController>();
        InitiativeList = new List<PawnController>();

        var pawnController = Application.Instance.PlayerManager.GetPawnController();
        pawnController.Init();
        
        var playerCanvasController = PawnCanvases[0];
        pawnController.PawnCanvasController = playerCanvasController;
        playerCanvasController.Init(pawnController);
        ActivePawns.Add(pawnController);

        foreach (var enemy in Enemies)
        {
            var enemyController = enemy.PawnController;
            
            enemyController.Init();
            
            if (enemy.IsBoss)
            {
                enemyController.PawnCanvasController = BossCanvas; 
            }
            
            enemyController.PawnCanvasController.Init(enemyController);
            
            EnemyPawns.Add(enemyController);
        }

        foreach (var alliedController in Application.Instance.PartyManager.Party)
        {
            alliedController.PlayerFollowController.StopFollow();
            alliedController.Init();
            
            var canvasController = PawnCanvases.First(c => !c.Initiated);
            alliedController.PawnCanvasController = canvasController;
            canvasController.Init(alliedController);
            
            ActivePawns.Add(alliedController);
        }

        PlayBattle();
    }

    public void PlayBattle()
    {
        BattleCanvas.gameObject.SetActive(true);

        var pawnsList = new List<PawnController>();
        pawnsList.AddRange(ActivePawns);
        pawnsList.AddRange(EnemyPawns);

        InitiativeList = pawnsList.OrderByDescending(p => p.Pawn.Initiative).ToList();

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
            foreach (var enemyPawn in EnemyPawns)
            {
                if (!enemyPawn.PawnState.AbleToFight)
                    continue;
                
                enemyPawn.Dance();
            }
            
            BattleLostCanvas.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(1f);
            
            foreach (var pawn in InitiativeList)
            {
                pawn.Deactivate();
            
                if(pawn.Team == TeamType.Enemies)
                    pawn.gameObject.SetActive(false);
            }
            
            Application.Instance.SceneManager.RespawnAtBonfire();
        }

        if (hasPlayers)
        {
            Application.Instance.AudioManager.PlayMusic(MusicType.Victory);

            foreach (var playerPawn in ActivePawns)
            {
                playerPawn.Dance();
            }

            yield return new WaitForSeconds(3f);

            foreach (var pawn in InitiativeList)
            {
                pawn.Deactivate();
            
                if(pawn.Team == TeamType.Enemies)
                    pawn.gameObject.SetActive(false);
            }
            
            Application.Instance.SceneManager.EndBattleScene(BattleId);
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
}