using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager: MonoBehaviour
{
    [field: SerializeField]
    public PlayerController PlayerController { get; private set; }
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
    
    public void SpawnPlayer()
    {
        PlayerController.GetComponent<PawnController>().enabled = false;
        PlayerController.gameObject.SetActive(true);
        PlayerController.enabled = true;
        
        var playerMovementController = PlayerController.GetComponent<PlayerMovementController>();
        playerMovementController.enabled = true;
        playerMovementController.Prepare();
    }
}