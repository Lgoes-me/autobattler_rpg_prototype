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

        var playerManager = Application.Instance.GetManager<PlayerManager>();
        await playerManager.MovePlayerTo(Destination);
        
        playerManager.EnablePlayerInput();
        
        Active = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            UseCorridor();
        }
    }

    private async void UseCorridor()
    {
        var playerManager = Application.Instance.GetManager<PlayerManager>();
        
        playerManager.DisablePlayerInput();
        await playerManager.MovePlayerTo(SpawnPoint);

        Application.Instance.GetManager<SceneManager>().ChangeContext(Spawn);
    }
}