using System.Collections;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private Battle Battle { get; set; }
    private CombatEncounterData CombatEncounter { get; set; }

    public void Init(string battleId, CombatEncounterData combatEncounter)
    {
        CombatEncounter = combatEncounter;
        
        Application.Instance.GetManager<PartyManager>().StopPartyFollow();
        
        Battle = new Battle(battleId);
        
        foreach (var enemy in CombatEncounter.Enemies)
        {
            var enemyController = enemy.PawnController;

            if (enemy.IsBoss(out var bossComponentData))
            {
                enemyController.RemoveCanvasController();

                var bossModifiers = bossComponentData.ToDomain();

                Application.Instance.GetManager<BattleEventsManager>().SetBoss(enemyController, bossModifiers);

                var bossCanvas = Application.Instance.GetManager<InterfaceManager>().BossCanvas;
                
                bossCanvas.Init(enemyController.Pawn);
                bossCanvas.SetModifiers(bossModifiers);
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

        var battleEventsManager = Application.Instance.GetManager<BattleEventsManager>();
        battleEventsManager.PrepareBattle(Battle);
        battleEventsManager.DoBattleStartEvent();

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
        RealizeTurn();
        
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

    private void RealizeTurn()
    {
        var initiativeList = Battle.Pawns;

        foreach (var pawn in initiativeList)
        {
            pawn.RealizeTurn();
        }
    }

    private IEnumerator OnDefeat()
    {
        foreach (var pawn in Battle.Pawns)
        {
            pawn.EndBattle();

            if (pawn.Pawn.Team == TeamType.Enemies)
            {
                if (!pawn.PawnState.AbleToFight)
                    continue;

                pawn.Dance();
            }
        }

        CombatEncounter.OnDefeat?.Invoke();
        
        Application.Instance.GetManager<InterfaceManager>().ShowDefeatCanvas();

        yield return new WaitForSeconds(1f);

        Application.Instance.GetManager<InterfaceManager>().HideDefeatCanvas();
        Application.Instance.GetManager<SceneManager>().RespawnAtBonfire();
    }

    private IEnumerator OnVictory()
    {
        Application.Instance.GetManager<AudioManager>().PlayMusic(MusicType.Victory);

        yield return new WaitForSeconds(1f);

        foreach (var pawn in Battle.Pawns)
        {
            pawn.EndBattle();

            if (pawn.Pawn.Team == TeamType.Enemies)
            {
                pawn.gameObject.SetActive(false);
            }
            else
            {
                pawn.Dance();
                pawn.Pawn.GetComponent<ResourceComponent>().EndOfBattleHeal();
                pawn.Pawn.GetComponent<ResourceComponent>().GiveXp(CombatEncounter.Experience);
            }
        }
        
        var battleEventsManager = Application.Instance.GetManager<BattleEventsManager>();
        battleEventsManager.DoBattleFinishedEvent();
        battleEventsManager.FinishBattle();

        yield return new WaitForSeconds(2f);
        
        foreach (var pawn in Battle.PlayerPawns)
        {
            pawn.Idle();
        }
        
        CombatEncounter.OnVictory?.Invoke();
        
        Application.Instance.GetManager<PlayerManager>().EnablePlayerInput();
        Application.Instance.GetManager<PartyManager>().SetPartyToFollow(false);
        
        var gameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        
        gameSaveManager.AddBattle(Battle);
        gameSaveManager.SaveCurrentGameState();
    }
}