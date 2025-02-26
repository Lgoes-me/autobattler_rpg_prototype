using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaController : MonoBehaviour
{
    [field: SerializeField] private List<EnemyInfo> Enemies { get; set; }
    [field: SerializeField] private BattleController BattleController { get; set; }
    [field: SerializeReference] [field: SerializeField] private GameAction EndBattleAction { get; set; }
    
    private string Id { get; set; }

    public void Init(string id)
    {
        Id = id;
        
        if (Application.Instance.GameSaveManager.ContainsBattle(Id))
        {
            gameObject.SetActive(false);
            return;
        }
        
        foreach (var enemy in Enemies)
        {
            enemy.PreparePawn();
            enemy.EnemyController.Init(this);
        }
    }

    public void StartBattle()
    {
        foreach (var enemy in Enemies)
        {
            enemy.EnemyController.Prepare();
        }
        
        Application.Instance.PlayerManager.PlayerToBattle();
        BattleController.ActivateBattleScene(Id, Enemies, EndBattleAction);
    }
}

[Serializable]
public class EnemyInfo
{
    [field: SerializeField] public bool IsBoss { get; set; }
    
    [field: SerializeField] public EnemyController EnemyController { get; set; }
    [field: SerializeField] private PawnData PawnData { get; set; }
    
    public PawnController PawnController => EnemyController.PawnController;

    public void PreparePawn()
    {
        var pawn = PawnData.ToDomain(TeamType.Enemies);
        PawnController.Init(pawn);
    }
}