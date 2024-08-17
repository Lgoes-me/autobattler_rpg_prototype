using UnityEngine;

public class DoorController : MonoBehaviour
{
    //Domain
    [field: SerializeField] public string DoorName { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] private string SceneName { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Application.Instance.SceneManager.UseDoorToChangeScene(DoorName, SceneName);
        }
    }
}
