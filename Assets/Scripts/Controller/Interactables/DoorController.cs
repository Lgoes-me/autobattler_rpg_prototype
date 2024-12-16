using System;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : CorridorAreaController, IInteractableListener
{
    [field: SerializeField] private NavMeshObstacle Obstacle { get; set; }
    [field: SerializeField] private Transform Door { get; set; }

    [field: SerializeField] private InteractableController Controller { get; set; }

    private void Awake()
    {
        Controller.Interactable = this;
    }
    
    public void Select(Action callback)
    {
        OpenDoor();
        UseCorridor();
    }
    
    public void UnSelect()
    {
        CloseDoor();
    }

    public override void SpawnPlayer(PlayerManager playerManager)
    {
        Controller.Enabled = false;
        OpenDoor();
        base.SpawnPlayer(playerManager);
    }

    protected override void OnArrive()
    {
        Controller.Enabled = true;
        CloseDoor();
    }

    private void OpenDoor()
    {
        Door.gameObject.SetActive(false);
        Obstacle.enabled = false;
    }

    private void CloseDoor()
    {
        Door.gameObject.SetActive(true);
        Obstacle.enabled = true;
    }
}