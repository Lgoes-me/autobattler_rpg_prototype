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

    private void InstantiatePlayer()
    {
        if(PawnController != null)
        {
            Destroy(PawnController.gameObject);
        }
        
        PlayerController = Instantiate(PlayerControllerPrefab, Vector3.zero, Quaternion.identity, transform);
        PlayerController.enabled = true;
        PlayerController.tag = "Player";
        
        PawnController = PlayerController.GetComponent<PawnController>();
        PawnController.enabled = false;
        
        NavMeshAgent = PlayerController.GetComponent<NavMeshAgent>();
    }

    public void SetNewPlayerPawn(Pawn pawn)
    {
        InstantiatePlayer();
        
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