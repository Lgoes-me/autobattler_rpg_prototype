using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Battle Battle { get; private set; }
    private GameAction EndBattleAction { get; set; }

    public void ActivateBattleScene(string battleId, List<EnemyInfo> enemies, GameAction endBattleAction)
    {
        EndBattleAction = endBattleAction;
        
        Application.Instance.AudioManager.PlayMusic(MusicType.Battle);

        var enemyPawns = new List<PawnController>();
        var playerPawns = new List<PawnController>();

        foreach (var enemy in enemies)
        {
            var enemyController = enemy.PawnController;
            enemyController.StartBattle(this);

            if (enemy.IsBoss)
            {
                enemyController.PawnCanvasController = Application.Instance.InterfaceManager.BossCanvas;
            }

            enemyController.PawnCanvasController.Init(enemyController.Pawn);
            enemyPawns.Add(enemyController);
        }
        
        Application.Instance.PartyManager.StopPartyFollow();

        foreach (var alliedController in Application.Instance.PartyManager.Party)
        {
            alliedController.StartBattle(this);
            playerPawns.Add(alliedController);
        }

        foreach (var playerPawn in playerPawns)
        {
            if (playerPawn.PawnCanvasController is not ProfileCanvasController profileCanvasController) 
                continue;
            
            profileCanvasController.StartBattle();
        }
        
        Battle = new Battle(battleId, enemyPawns, playerPawns);
        Application.Instance.BattleEventsManager.DoBattleStartEvent(Battle);

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
            yield return pawn.PawnTurn();
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

        Application.Instance.InterfaceManager.ShowDefeatCanvas();

        yield return new WaitForSeconds(1f);

        Application.Instance.InterfaceManager.HideDefeatCanvas();
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
            if (playerPawn.PawnCanvasController is not ProfileCanvasController profileCanvasController) 
                continue;
            
            profileCanvasController.EndBattle();
        }

        foreach (var pawn in Battle.Pawns)
        {
            pawn.FinishBattle();

            if (pawn.Pawn.Team == TeamType.Enemies)
                pawn.gameObject.SetActive(false);
        }

        var roomScene = FindObjectOfType<RoomScene>();

        Application.Instance.AudioManager.PlayMusic(roomScene.Music);
        Application.Instance.PlayerManager.PlayerToWorld();
        Application.Instance.PartyManager.SetPartyToFollow(false);
        Application.Instance.GameSaveManager.SaveBattle(Battle);

        EndBattleAction?.Invoke();
    }
}