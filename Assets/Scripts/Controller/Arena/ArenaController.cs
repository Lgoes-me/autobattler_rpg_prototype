using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [field: SerializeField] private GameObject PreBattleCanvas { get; set; }
    [field: SerializeField] private GameObject BattleCanvas { get; set; }
    [field: SerializeField] private GameObject BattleLostCanvas { get; set; }
    [field: SerializeField] private List<PawnCanvasController> PawnCanvases { get; set; }
    
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
            pawnController.Init();
            EnemyPawns.Add(pawnController);
        }

        var party = Application.Instance.PartyManager.SelectedParty;
        
        foreach (var alliedController in party)
        {
            alliedController.PlayerFollowController.StopFollow();
            alliedController.Init(PawnCanvases.First(c => !c.Initiated));
            ActivePawns.Add(alliedController);
        }
    }

    private void SpawnPlayerPawn()
    {
        var pawnController = Application.Instance.PlayerManager.GetPawnController();
        pawnController.Init(PawnCanvases[0]);
        ActivePawns.Add(pawnController);
    }

    public void PlayBattle()
    {
        PreBattleCanvas.gameObject.SetActive(false);
        BattleCanvas.gameObject.SetActive(true);

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
            foreach (var enemyPawn in EnemyPawns)
            {
                if (!enemyPawn.PawnState.AbleToFight)
                    continue;
                
                enemyPawn.Dance();
            }
            
            BattleLostCanvas.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(2f);
            
            foreach (var pawn in InitiativeList)
            {
                pawn.Deactivate();
            
                if(pawn.Team == TeamType.Enemies)
                    pawn.gameObject.SetActive(false);
            }
            
            SceneManager.RespawnAtBonfire();
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

            Application.Instance.PlayerManager.AddDefeated(BattleId);
            SceneManager.EndBattleScene();
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