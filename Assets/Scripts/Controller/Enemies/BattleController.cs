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
        
        Application.Instance.GetManager<PartyManager>().StopPartyFollow();

        var enemyPawns = new List<PawnController>();
        var playerPawns = new List<PawnController>();

        foreach (var enemy in enemies)
        {
            var enemyController = enemy.PawnController;

            if (enemy.IsBoss)
            {
                enemyController.RemoveCanvasController();
                Application.Instance.GetManager<InterfaceManager>().BossCanvas.Init(enemyController);
            }

            enemyPawns.Add(enemyController);
        }

        foreach (var alliedController in Application.Instance.GetManager<PartyManager>().Party)
        {
            playerPawns.Add(alliedController);
        }

        Battle = new Battle(battleId, enemyPawns, playerPawns);

        foreach (var pawnController in Battle.Pawns)
        {
            pawnController.StartBattle(this, Battle);
        }
        
        Application.Instance.GetManager<BattleEventsManager>().DoBattleStartEvent(Battle);

        StartCoroutine(BattleCoroutine());
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
        var initiativeList = Battle.GetInitiativeList();

        foreach (var pawn in initiativeList)
        {
            pawn.RealizaTurno();
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
            playerPawn.ReceiveHeal(true);
        }

        yield return new WaitForSeconds(1f);

        foreach (var pawn in Battle.Pawns)
        {
            pawn.FinishBattle();

            if (pawn.Pawn.Team == TeamType.Enemies)
                pawn.gameObject.SetActive(false);
        }

        Application.Instance.GetManager<PlayerManager>().EnablePlayerInput();
        Application.Instance.GetManager<PartyManager>().SetPartyToFollow(false);
        Application.Instance.GetManager<GameSaveManager>().SaveBattle(Battle);

        EndBattleAction?.Invoke();
    }
}