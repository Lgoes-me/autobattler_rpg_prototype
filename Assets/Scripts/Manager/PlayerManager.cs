using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public PawnController PawnController { get; private set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }

    private GameSaveManager GameSaveManager { get; set; }
    
    private void Start()
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

    public void SpawnPlayerAt(Transform location)
    {
        NavMeshAgent.enabled = false;
        PlayerController.transform.position = location.position;
        PawnController.CharacterController.SetDirection(location.forward);
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