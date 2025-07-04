using System;
using System.Linq;
using UnityEngine;

public class EnemyAreaController : MonoBehaviour
{
    [field: SerializeField] private EnemyController[] EnemyControllers { get; set; }
    [field: SerializeField] private BattleController BattleController { get; set; }
    
    private CombatEncounterData CombatEncounter { get; set; }
    
    private string Id { get; set; }

    public void Init(string sceneId, CombatEncounterData combatEncounter)
    {
        Id = $"{gameObject.name}-{sceneId}";
        CombatEncounter = combatEncounter;
        
        if (Application.Instance.GetManager<GameSaveManager>().ContainsBattle(Id))
        {
            gameObject.SetActive(false);
            return;
        }

        for (var index = 0; index < CombatEncounter.Enemies.Count; index++)
        {
            var enemy = CombatEncounter.Enemies[index];
            var enemyController = EnemyControllers[index];
            enemy.PreparePawn(enemyController.PawnController, TeamType.Enemies);
            enemyController.Init(this);
        }
    }

    public void StartBattle()
    {
        foreach (var enemy in EnemyControllers)
        {
            enemy.Prepare();
        }
        
        Application.Instance.GetManager<PlayerManager>().DisablePlayerInput();
        BattleController.ActivateBattleScene(Id, CombatEncounter);
    }
    
    private void OnValidate()
    {
        if(Id != string.Empty)
            return;
        
        Id = Guid.NewGuid().ToString();
    }


    public CombatEncounterData ToCombatEncounterData()
    {
        var enemies = EnemyControllers.Select(e => e.ToEnemyData()).ToList();

        return new CombatEncounterData()
        {
            Id = Id,
            Enemies = enemies,
        };
    }
}