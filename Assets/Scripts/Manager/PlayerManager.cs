using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] private PawnController PawnController { get; set; }
    [field: SerializeField] private PlayerMovementController PlayerMovementController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    public List<string> Defeated { get; private set; }

    private void Start()
    {
        Defeated = new List<string>();
    }

    public void AddDefeated(string enemy)
    {
        Defeated.Add(enemy);
    }

    public void SpawnPlayerAt(Transform location)
    {
        NavMeshAgent.enabled = false;
        PlayerController.transform.position = location.position;
        PlayerController.transform.forward = location.forward;
        NavMeshAgent.enabled = true;
    }

    public void PlayerToBattle()
    {
        PlayerController.enabled = false;

        PlayerMovementController.enabled = false;
        PlayerMovementController.Prepare();

        PawnController.enabled = true;
        NavMeshAgent.enabled = true;
    }

    public void PlayerToWorld()
    {
        PawnController.Deactivate();
        PlayerController.gameObject.SetActive(true);
        PlayerController.enabled = true;

        PlayerMovementController.enabled = true;
        PlayerMovementController.Prepare();
    }

    public void SetNewCameraPosition(Transform cameraTransform)
    {
        PlayerMovementController.SetNewCameraPosition(cameraTransform);
    }
    
    public PawnController GetPawnController(ArenaController arenaController)
    {
        enabled = false;
        PawnController.Init(arenaController, PawnData.ToDomain());
        return PawnController;
    }
}