using System.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IManager
{
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
        PlayerController.enabled = false;
    }

    public void EnablePlayerInput()
    {
        PlayerController.enabled = true;
    }

    public async Task MovePlayerTo(Transform destination)
    {
        await PlayerController.MovePlayerTo(destination);
    }
}