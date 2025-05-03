using UnityEngine;

public class CorridorAreaController : SpawnController
{
    [field: SerializeField] private Transform Destination { get; set; }

    public SpawnDomain Spawn { get; set; }
    private bool CanUse { get; set; } = true;

    public override async void SpawnPlayer()
    {
        base.SpawnPlayer();
        CanUse = false;

        var playerManager = Application.Instance.GetManager<PlayerManager>();
        
        await playerManager.MovePlayerTo(Destination);
        playerManager.EnablePlayerInput();
        CanUse = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Spawn.Active && CanUse && other.CompareTag("Player"))
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