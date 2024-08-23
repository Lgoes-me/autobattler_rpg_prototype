using System.Collections.Generic;
using UnityEngine;

public class PlayerManager: MonoBehaviour
{
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
        var player = FindObjectOfType<PlayerController>().transform;
        player.position = location.position;
        player.forward = location.forward;
    }
    
    public void SpawnPlayer()
    {
        var player = FindObjectOfType<PlayerController>();
        player.GetComponent<PawnController>().enabled = false;

        player.gameObject.SetActive(true);
        
        player.enabled = true;
        
        var playerMovementController = player.GetComponent<PlayerMovementController>();
        playerMovementController.enabled = true;
        playerMovementController.Prepare();
    }
}