using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] private PlayerController PlayerControllerPrefab { get; set; }
    
    public PlayerController PlayerController { get; private set; }
    public PawnController PawnController { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }

    private GameSaveManager GameSaveManager { get; set; }

    public void Prepare()
    {
        GameSaveManager = Application.Instance.GameSaveManager;
    }
    
    public void SetNewPlayerPawn(PawnController pawnController)
    {
        PawnController = pawnController;
        PawnController.tag = "Player";
        PawnController.enabled = false;

        PlayerController = PawnController.GetComponent<PlayerController>();
        PlayerController.enabled = true;
        
        NavMeshAgent = PlayerController.GetComponent<NavMeshAgent>();

        PlayerController.Init();
    }

    public void SpawnPlayerAt(Transform spawnPoint)
    {
        NavMeshAgent.enabled = false;
        PlayerController.transform.position = spawnPoint.position;
        PawnController.CharacterController.SetDirection(spawnPoint.forward);
        NavMeshAgent.enabled = true;
    }

    public void PlayerToBattle()
    {
        PlayerController.enabled = false;
        PawnController.enabled = true;
    }

    public void PlayerToWorld()
    {
        NavMeshAgent.isStopped = true;
        
        PlayerController.enabled = true;
        PawnController.enabled = false;
    }
}