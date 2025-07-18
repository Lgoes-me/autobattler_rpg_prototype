using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IManager
{
    public Stats PlayerStats { get; private set; }
    public Transform PlayerTransform => PlayerController.transform;
    private PlayerController PlayerController { get; set; }

    //Deve ser resetado em caso de mudança de save
    private void Awake()
    {
        PlayerStats = new Stats(new Dictionary<Stat, int>());
    }

    public void SetNewPlayerPawn(PawnController pawnController)
    {
        pawnController.tag = "Player";

        PlayerController = pawnController.GetComponent<PlayerController>();
        PlayerController.enabled = true;

        PlayerController.Init();
    }

    public void DisablePlayerInput()
    {
        if (PlayerController == null)
            return;

        PlayerController.Disable();
    }

    public void EnablePlayerInput()
    {
        if (PlayerController == null)
            return;

        PlayerController.Enable();
    }

    public Task MovePlayerTo(Transform destination)
    {
        return PlayerController.MovePlayerTo(destination);
    }

    public void SetDestination(Vector3 mouseInput)
    {
        if (PlayerController == null)
            return;

        PlayerController.SetDestination(mouseInput);
    }
}