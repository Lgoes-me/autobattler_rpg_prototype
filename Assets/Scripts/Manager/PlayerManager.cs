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
        if(PlayerController == null)
            return;
        
        PlayerController.Disable();
    }

    public void EnablePlayerInput()
    {
        if(PlayerController == null)
            return;

        PlayerController.Enable();
    }

    public Task MovePlayerTo(Transform destination)
    {
        return PlayerController.MovePlayerTo(destination);
    }
    
    public void SetDestination(Vector3 mouseInput)
    {
        if(PlayerController == null)
            return;
        
        PlayerController.SetDestination(mouseInput);
    }
}