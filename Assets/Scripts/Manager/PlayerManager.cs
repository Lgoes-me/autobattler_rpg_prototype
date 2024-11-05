using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] private GameSaveManager GameSaveManager { get; set; }
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public PawnController PawnController { get; private set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
   
    public void SetNewPlayerPawn(PawnData pawnData)
    {
        if(PawnController.transform.childCount > 0)
            Destroy(PawnController.transform.GetChild(0).gameObject);
        
        var pawn = pawnData.ToDomain();
        GameSaveManager.ApplyPawnInfo(pawn);
        
        PawnController.Init(pawn);
        PlayerController.Init();
    }

    public void SpawnPlayerAt(Transform location)
    {
        NavMeshAgent.enabled = false;
        PlayerController.transform.position = location.position;
        PlayerController.CharacterController.SetDirection(location.forward);
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