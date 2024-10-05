using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private List<PawnController> PlayerPawns { get; set; }
    private List<PawnController> EnemyPawns { get; set; }
    private List<PawnController> InitiativeList { get; set; }

    private string BattleId { get; set; }

    public void ActivateBattleScene(string battleId, List<EnemyInfo> enemies)
    {
        Application.Instance.AudioManager.PlayMusic(MusicType.Battle);
        
        BattleId = battleId;
        
        PlayerPawns = new List<PawnController>();
        EnemyPawns = new List<PawnController>();
        InitiativeList = new List<PawnController>();

        foreach (var enemy in enemies)
        {
            var enemyController = enemy.PawnController;
            enemyController.Init();
            
            if (enemy.IsBoss)
            {
                enemyController.PawnCanvasController = Application.Instance.BossCanvas; 
            }
            
            enemyController.PawnCanvasController.Init(enemyController);
            EnemyPawns.Add(enemyController);
        }

        var playerPawnController = Application.Instance.PlayerManager.GetPawnController();
        playerPawnController.Init();

        var pawnCanvases = Application.Instance.PawnCanvases;
        
        var playerCanvasController = pawnCanvases[0];
        playerPawnController.PawnCanvasController = playerCanvasController;
        playerCanvasController.Init(playerPawnController);
        PlayerPawns.Add(playerPawnController);

        foreach (var alliedController in Application.Instance.PartyManager.Party)
        {
            alliedController.PlayerFollowController.StopFollow();
            alliedController.Init();
            
            var canvasController = pawnCanvases.First(c => !c.Initiated);
            alliedController.PawnCanvasController = canvasController;
            canvasController.Init(alliedController);
            
            PlayerPawns.Add(alliedController);
        }

        var pawnsList = new List<PawnController>();
        pawnsList.AddRange(PlayerPawns);
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
            hasPlayers = PlayerPawns.Count(e => e.PawnState.AbleToFight) > 0;
        }

        if (hasEnemies)
        {
            yield return OnDefeat();
        }

        if (hasPlayers)
        {
            yield return OnVictory();
        }
    }

    private IEnumerator OnDefeat()
    {
        foreach (var enemyPawn in EnemyPawns)
        {
            if (!enemyPawn.PawnState.AbleToFight)
                continue;

            enemyPawn.Dance();
        }

        foreach (var pawn in InitiativeList)
        {
            pawn.PawnCanvasController.Hide();
        }

        Application.Instance.ShowDefeatCanvas();

        yield return new WaitForSeconds(1f);

        Application.Instance.HideDefeatCanvas();
        Application.Instance.SceneManager.RespawnAtBonfire();
    }

    private IEnumerator RealizaTurno()
    {
        foreach (var pawn in InitiativeList)
        {
            if (!pawn.PawnState.CanTakeTurn) continue;
            yield return pawn.Turno(InitiativeList);
        }
    }

    private IEnumerator OnVictory()
    {
        Application.Instance.AudioManager.PlayMusic(MusicType.Victory);

        foreach (var playerPawn in PlayerPawns)
        {
            playerPawn.Dance();
        }

        yield return new WaitForSeconds(1f);

        foreach (var playerPawn in PlayerPawns)
        {
            var health = Mathf.Clamp(playerPawn.Pawn.Health + 15, 0, playerPawn.Pawn.MaxHealth);
            playerPawn.Pawn.Health += health;
            playerPawn.ReeceiveHeal(true);
        }

        yield return new WaitForSeconds(1f);

        foreach (var playerPawn in PlayerPawns)
        {
            playerPawn.PawnCanvasController.Hide();
        }

        foreach (var pawn in InitiativeList)
        {
            pawn.Deactivate();

            if (pawn.Team == TeamType.Enemies)
                pawn.gameObject.SetActive(false);
        }

        var roomScene = FindObjectOfType<RoomScene>();

        Application.Instance.AudioManager.PlayMusic(roomScene.Music);

        Application.Instance.PlayerManager.PlayerToWorld();

        var save = Application.Instance.Save;
        save.PlayerPawn = Application.Instance.PlayerManager.PawnController.Pawn.GetPawnInfo();
        save.SelectedParty = Application.Instance.PartyManager.Party.ToDictionary(p => p.Pawn.Id, p => p.Pawn.GetPawnInfo());
        save.DefeatedEnemies.Add(BattleId);
        Application.Instance.SaveManager.SaveData(save);
    }

}