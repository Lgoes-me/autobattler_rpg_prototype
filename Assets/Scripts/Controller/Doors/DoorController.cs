using UnityEngine;

public class DoorController : SpawnController
{
    [field: SerializeField] private string SceneDestination { get; set; }
    [field: SerializeField] private bool Active { get; set; } = true;
    
    [field: SerializeField] private Transform Destination { get; set; }

    public override void SpawnPlayer(PlayerManager playerManager)
    {
        base.SpawnPlayer(playerManager);
        Active = false;
        
        playerManager.NavMeshAgent.SetDestination(Destination.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Active = true;
            Application.Instance.PlayerManager.NavMeshAgent.isStopped = true;
            Application.Instance.PlayerManager.NavMeshAgent.ResetPath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            Application.Instance.SceneManager.UseDoorToChangeScene(Id, SceneDestination);
        }
    }
}