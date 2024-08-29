using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public PawnController PawnController { get; private set; }
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
        PlayerController.GetComponent<NavMeshAgent>().enabled = false;
        PlayerController.transform.position = location.position;
        PlayerController.transform.forward = location.forward;
        PlayerController.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void PlayerToBattle()
    {
        PlayerController.enabled = false;

        var playerMovementController = PlayerController.GetComponent<PlayerMovementController>();
        playerMovementController.enabled = false;
        playerMovementController.Prepare();

        PawnController.enabled = true;
        PlayerController.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void PlayerToWorld()
    {
        PlayerController.GetComponent<PawnController>().Deactivate();
        PlayerController.gameObject.SetActive(true);
        PlayerController.enabled = true;

        var playerMovementController = PlayerController.GetComponent<PlayerMovementController>();
        playerMovementController.enabled = true;
        playerMovementController.Prepare();
    }

    public void SetNewCameraPosition(Transform cameraTransform)
    {
        var playerMovementController = PlayerController.GetComponent<PlayerMovementController>();
        playerMovementController.SetNewCameraPosition(cameraTransform);
    }
}