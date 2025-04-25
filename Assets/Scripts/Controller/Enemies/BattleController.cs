using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Battle Battle { get; private set; }
    private GameAction EndBattleAction { get; set; }

    public void ActivateBattleScene(string battleId, List<EnemyData> enemies, GameAction endBattleAction)
    {
        EndBattleAction = endBattleAction;
        
        Application.Instance.GetManager<PartyManager>().StopPartyFollow();
        
        Battle = new Battle(battleId);
        
        foreach (var enemy in enemies)
        {
            var enemyController = enemy.PawnController;

            if (enemy.IsBoss)
            {
                enemyController.RemoveCanvasController();
                Application.Instance.GetManager<InterfaceManager>().BossCanvas.Init(enemyController.Pawn);
            }

            AddPawn(enemyController, TeamType.Enemies);
        }

        foreach (var alliedController in Application.Instance.GetManager<PartyManager>().Party)
        {
            AddPawn(alliedController, TeamType.Player);
        }

        foreach (var pawnController in Battle.Pawns)
        {
            pawnController.StartBattle(Battle);
        }
        
        Application.Instance.GetManager<BattleEventsManager>().DoBattleStartEvent(Battle);
        Application.Instance.GetManager<InterfaceManager>().StartBattle();

        StartCoroutine(BattleCoroutine());
    }

    private void AddPawn(PawnController pawnController, TeamType team)
    {
        switch (team)
        {
            case TeamType.Player:
                Battle.AddPlayerPawn(pawnController);
                break;
            case TeamType.Enemies:
                Battle.AddEnemy(pawnController);
                break;
        }
    }

    private IEnumerator BattleCoroutine()
    {
        RealizaTurno();
        
        yield return new WaitUntil(() => !Battle.HasEnemies || !Battle.HasPlayers);

        if (Battle.HasEnemies)
        {
            yield return OnDefeat();
        }

        if (Battle.HasPlayers)
        {
            yield return OnVictory();
        }
    }

    private void RealizaTurno()
    {
        var initiativeList = Battle.Pawns;

        foreach (var pawn in initiativeList)
        {
            pawn.RealizeTurn();
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

        Application.Instance.GetManager<InterfaceManager>().ShowDefeatCanvas();

        yield return new WaitForSeconds(1f);

        Application.Instance.GetManager<InterfaceManager>().HideDefeatCanvas();
        Application.Instance.GetManager<SceneManager>().RespawnAtBonfire();
    }

    private IEnumerator OnVictory()
    {
        Application.Instance.GetManager<AudioManager>().PlayMusic(MusicType.Victory);

        foreach (var playerPawn in Battle.PlayerPawns)
        {
            playerPawn.Dance();
        }

        yield return new WaitForSeconds(1f);

        foreach (var playerPawn in Battle.PlayerPawns)
        {
            playerPawn.Pawn.EndOfBattleHeal();
        }

        yield return new WaitForSeconds(1f);

        foreach (var pawn in Battle.Pawns)
        {
            pawn.FinishBattle();

            if (pawn.Pawn.Team == TeamType.Enemies)
                pawn.gameObject.SetActive(false);
        }
        
        Application.Instance.GetManager<InterfaceManager>().FinishBattle();
        Application.Instance.GetManager<PlayerManager>().EnablePlayerInput();
        Application.Instance.GetManager<PartyManager>().SetPartyToFollow(false);
        Application.Instance.GetManager<GameSaveManager>().SaveBattle(Battle);

        EndBattleAction?.Invoke();
    }
}