using UnityEngine;

public class CorridorAreaController : SpawnController
{
    [field: SerializeField] private string SceneDestination { get; set; }
    [field: SerializeField] private bool Active { get; set; } = true;
    
    [field: SerializeField] private Transform Destination { get; set; }

    public override async void SpawnPlayer(PlayerManager playerManager)
    {
        base.SpawnPlayer(playerManager);
        Active = false;
        
        await this.WaitToArriveAtDestination(playerManager.NavMeshAgent, Destination.position);
        OnArrive();
    }
    
    protected virtual void OnArrive()
    {
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
        await this.WaitToArriveAtDestination(Application.Instance.PlayerManager.NavMeshAgent, Spawn.position);
        Application.Instance.SceneManager.UseDoorToChangeScene(Id, SceneDestination);
    }
}