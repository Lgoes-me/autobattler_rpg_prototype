using UnityEngine;

public class CorridorAreaController : SpawnController
{
    [field: SerializeField] private bool Active { get; set; } = true;

    [field: SerializeField] private Transform Destination { get; set; }

    public SpawnDomain Spawn { get; set; }

    public override async void SpawnPlayer()
    {
        base.SpawnPlayer();
        Active = false;

        await this.WaitToArriveAtDestination(Application.Instance.PlayerManager.NavMeshAgent, Destination.position);
        
        Application.Instance.PlayerManager.EnablePlayerInput();
        
        Active = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            UseCorridor();
        }
    }

    protected async void UseCorridor()
    {
        Application.Instance.PlayerManager.DisablePlayerInput();
        await this.WaitToArriveAtDestination(Application.Instance.PlayerManager.NavMeshAgent, SpawnPoint.position);
        Application.Instance.SceneManager.ChangeContext(Spawn);
    }
}