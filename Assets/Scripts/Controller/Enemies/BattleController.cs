using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private Battle Battle { get; set; }

    public void ActivateBattleScene(string battleId, List<EnemyInfo> enemies)
    {
        Application.Instance.AudioManager.PlayMusic(MusicType.Battle);
        
        var enemyPawns = new List<PawnController>();
        var playerPawns = new List<PawnController>();

        foreach (var enemy in enemies)
        {
            var enemyController = enemy.PawnController;
            enemyController.Init();
            
            if (enemy.IsBoss)
            {
                enemyController.PawnCanvasController = Application.Instance.BossCanvas; 
            }
            
            enemyController.PawnCanvasController.Init(enemyController);
            enemyPawns.Add(enemyController);
        }

        var playerPawnController = Application.Instance.PlayerManager.GetPawnController();
        playerPawnController.Init();

        var pawnCanvases = Application.Instance.PawnCanvases;
        
        var playerCanvasController = pawnCanvases[0];
        playerPawnController.PawnCanvasController = playerCanvasController;
        playerCanvasController.Init(playerPawnController);
        playerPawns.Add(playerPawnController);

        foreach (var alliedController in Application.Instance.PartyManager.Party)
        {
            alliedController.PlayerFollowController.StopFollow();
            alliedController.Init();
            
            var canvasController = pawnCanvases.First(c => !c.Initiated);
            alliedController.PawnCanvasController = canvasController;
            canvasController.Init(alliedController);
            
            playerPawns.Add(alliedController);
        }

        Battle = new Battle(battleId, enemyPawns, playerPawns);

        StartCoroutine(BattleCoroutine());
    }
    
    private IEnumerator BattleCoroutine()
    {
        var hasEnemies = true;
        var hasPlayers = true;

        while (hasEnemies && hasPlayers)
        {
            yield return RealizaTurno();

            hasEnemies = Battle.HasEnemies;
            hasPlayers = Battle.HasPlayers;
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

    private IEnumerator RealizaTurno()
    {
        var initiativeList = Battle.GetInitiativeList();
        
        foreach (var pawn in initiativeList)
        {
            if (!pawn.PawnState.CanTakeTurn) continue;
            yield return pawn.Turno(Battle);
        }
    }

    private IEnumerator OnDefeat()
    {
        foreach (var enemyPawn in Battle.EnemyPawns)
        {
            if (!enemyPawn.PawnState.AbleToFight)
                continue;

            enemyPawn.Dance();
        }

        foreach (var pawn in Battle.Pawns)
        {
            pawn.PawnCanvasController.Hide();
        }

        Application.Instance.ShowDefeatCanvas();

        yield return new WaitForSeconds(1f);

        Application.Instance.HideDefeatCanvas();
        Application.Instance.SceneManager.RespawnAtBonfire();
    }

    private IEnumerator OnVictory()
    {
        Application.Instance.AudioManager.PlayMusic(MusicType.Victory);

        foreach (var playerPawn in Battle.PlayerPawns)
        {
            playerPawn.Dance();
        }

        yield return new WaitForSeconds(1f);

        foreach (var playerPawn in Battle.PlayerPawns)
        {
            playerPawn.Pawn.EndOfBattleHeal();
            playerPawn.ReceiveHeal(true);
        }

        yield return new WaitForSeconds(1f);

        foreach (var playerPawn in Battle.PlayerPawns)
        {
            playerPawn.PawnCanvasController.Hide();
        }

        foreach (var pawn in Battle.Pawns)
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
        save.DefeatedEnemies.Add(Battle.Id);
        Application.Instance.SaveManager.SaveData(save);
    }

}