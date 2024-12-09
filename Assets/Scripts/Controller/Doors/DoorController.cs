using Controller.Doors;
using UnityEngine;

public class DoorController : SpawnController
{
    [field: SerializeField] private string SceneDestination { get; set; }
    [field: SerializeField] private bool Active { get; set; } = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            Application.Instance.SceneManager.UseDoorToChangeScene(Id, SceneDestination);
        }
    }
}