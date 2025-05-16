using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaController : MonoBehaviour
{
    [field: SerializeField] public List<EnemyController> EnemyControllers { get; private set; }
    [field: SerializeField] private List<EnemyData> Enemies { get; set; }
    [field: SerializeField] private BattleController BattleController { get; set; }
    [field: SerializeReference] [field: SerializeField] private GameAction EndBattleAction { get; set; }
    
    private string Id { get; set; }

    public void Init(string sceneId)
    {
        Id = $"{gameObject.name}-{sceneId}";
        
        if (Application.Instance.GetManager<GameSaveManager>().ContainsBattle(Id))
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
        
        Application.Instance.GetManager<PlayerManager>().DisablePlayerInput();
        BattleController.ActivateBattleScene(Id, Enemies, EndBattleAction);
    }
    
    private void OnValidate()
    {
        if(Id != string.Empty)
            return;
        
        Id = Guid.NewGuid().ToString();
    }
}

[Serializable]
public class EnemyData
{
    [field: SerializeField] public bool IsBoss { get; set; }
    [field: SerializeField] public EnemyController EnemyController { get; set; }
    [field: SerializeField] public EnemyInfo EnemyInfo { get; set; }
    [field: SerializeField] private string Animation { get; set; }
    [field: SerializeField] private bool Forward { get; set; }
    
    public PawnController PawnController => EnemyController.PawnController;
    
    public void PreparePawn()
    {
        PawnController.Init(EnemyInfo.ToDomain(TeamType.Enemies));
        PawnController.CharacterController.SetDirection((Forward ? 1 : -1) * PawnController.transform.forward);
        PawnController.CharacterController.SetAnimationState(new DefaultState(Animation,
            () => { PawnController.CharacterController.SetAnimationState(new IdleState()); }));
    }
}

[Serializable]
public class EnemyInfo
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private int Level { get; set; } = 1;

    public Pawn ToDomain(TeamType teamType)
    {
        return PawnData.ToDomain(PawnStatus.Enemy, teamType, Level);
    }
}