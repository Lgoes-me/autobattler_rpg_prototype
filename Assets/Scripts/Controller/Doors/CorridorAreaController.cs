using UnityEngine;

public class CorridorAreaController : SpawnController
{
    [field: SerializeField] private string SceneDestination { get; set; }
    [field: SerializeField] private bool Active { get; set; } = true;
    
    [field: SerializeField] private Transform Destination { get; set; }

    public override void SpawnPlayer(PlayerManager playerManager)
    {
        base.SpawnPlayer(playerManager);
        Active = false;
        
        playerManager.SetDestination(Destination, OnArrive);
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

    protected void UseCorridor()
    {
        Application.Instance.PlayerManager.SetDestination(Spawn, () =>
        {
            Application.Instance.SceneManager.UseDoorToChangeScene(Id, SceneDestination);
        });
    }
}