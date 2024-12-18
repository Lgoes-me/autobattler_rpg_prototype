using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public PawnController PawnController { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

    private GameSaveManager GameSaveManager { get; set; }

    public void Prepare()
    {
        GameSaveManager = Application.Instance.GameSaveManager;
    }

    public void SetNewPlayerPawn(Pawn pawn)
    {
        if(PawnController.transform.childCount > 0)
            Destroy(PawnController.transform.GetChild(0).gameObject);
        
        GameSaveManager.ApplyPawnInfo(pawn);
        
        PawnController.Init(pawn);
        PlayerController.Init();
    }

    public void SpawnPlayerAt(SpawnController spawn)
    {
        NavMeshAgent.enabled = false;
        spawn.SpawnPlayer(this);
        NavMeshAgent.enabled = true;
    }

    public void PlayerToBattle()
    {
        PlayerController.enabled = false;
        PlayerController.Prepare();

        PawnController.enabled = true;
        NavMeshAgent.enabled = true;
    }

    public void PlayerToWorld()
    {
        NavMeshAgent.isStopped = true;
        
        PawnController.FinishBattle();
        PlayerController.gameObject.SetActive(true);
        PlayerController.enabled = true;
        PlayerController.Prepare();
    }
}