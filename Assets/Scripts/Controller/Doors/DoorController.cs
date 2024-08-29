using UnityEngine;

public class DoorController : MonoBehaviour
{
    [field: SerializeField] public string DoorName { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] private string SceneName { get; set; }
    
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Application.Instance.SceneManager.UseDoorToChangeScene(DoorName, SceneName);
        }
    }

    public void ActivateCameraArea()
    {
        CameraArea.ActivateCamera();
    }
}
