using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public PlayerController PlayerController { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    
    private GameSaveManager GameSaveManager { get; set; }

    public void Prepare()
    {
        GameSaveManager = Application.Instance.GameSaveManager;
    }
    
    public void SetNewPlayerPawn(PawnController pawnController)
    {
        pawnController.tag = "Player";

        PlayerController = pawnController.GetComponent<PlayerController>();
        PlayerController.enabled = true;
        
        NavMeshAgent = pawnController.GetComponent<NavMeshAgent>();
        
        PlayerController.Init();
    }

    public void DisablePlayerInput()
    {
        PlayerController.enabled = false;
    }

    public void EnablePlayerInput()
    {
        PlayerController.enabled = true;
    }
}