using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] private GameSaveManager GameSaveManager { get; set; }
    [field: SerializeField] private PartyManager PartyManager { get; set; }
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public PawnController PawnController { get; private set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
   
    public void Init()
    {
        var playerPawn = GameSaveManager.GetPlayerPawn();
        var pawnData = PartyManager.AvailableParty.First(p => p.Id == playerPawn.PawnName);

        SetNewPlayerPawn(pawnData);
    }

    public void SetNewPlayerPawn(PawnData pawn)
    {
        GameSaveManager.SetPlayerPawn(pawn);
        
        if(PawnController.transform.childCount > 0)
            Destroy(PawnController.transform.GetChild(0).gameObject);
        
        PawnController.SetCharacter(pawn);
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
        
        PawnController.Deactivate();
        PlayerController.gameObject.SetActive(true);
        PlayerController.enabled = true;
        PlayerController.Prepare();
        
        Application.Instance.PartyManager.SetPartyToFollow(false);
    }

    public PawnController GetPawnController()
    {
        enabled = false;
        return PawnController;
    }
}