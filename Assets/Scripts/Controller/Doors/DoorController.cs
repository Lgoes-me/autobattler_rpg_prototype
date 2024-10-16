using UnityEngine;

public class DoorController : MonoBehaviour
{
    [field: SerializeField] private string SceneName { get; set; }
    [field: SerializeField] public string DoorName { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] private bool Active { get; set; } = true;
    
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            var spawn = new SpawnDomain(DoorName, SceneName);
            Application.Instance.SceneManager.UseDoorToChangeScene(spawn);
        }
    }

    public void ActivateCameraArea()
    {
        CameraArea.ActivateCamera();
    }
}
