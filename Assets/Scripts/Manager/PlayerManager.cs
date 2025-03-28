using UnityEngine;

public class PlayerManager : MonoBehaviour, IManager
{
    public PlayerController PlayerController { get; private set; }

    public void SetNewPlayerPawn(PawnController pawnController)
    {
        pawnController.tag = "Player";

        PlayerController = pawnController.GetComponent<PlayerController>();
        PlayerController.enabled = true;
        
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