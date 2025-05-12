﻿using System.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IManager
{
    [field: SerializeField] private PawnController PawnController { get; set; }
    
    public Transform PlayerTransform => PlayerController.transform;
    
    private PlayerController PlayerController { get; set; }
    
    public void SetNewPlayerPawn(PawnController pawnController)
    {
        pawnController.tag = "Player";

        PlayerController = pawnController.GetComponent<PlayerController>();
        PlayerController.enabled = true;
        
        PlayerController.Init();
    }

    public void DisablePlayerInput()
    {
        PlayerController.Disable();
    }

    public void EnablePlayerInput()
    {
        PlayerController.Enable();
    }

    public async Task MovePlayerTo(Transform destination)
    {
        await PlayerController.MovePlayerTo(destination);
    }
    
    public void SetDestination(Vector3 mouseInput)
    {
        if(PlayerController == null)
            return;
        
        PlayerController.SetDestination(mouseInput);
    }
}