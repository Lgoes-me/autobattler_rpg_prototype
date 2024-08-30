using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [field: SerializeField] public string DoorName { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] private string SceneName { get; set; }
    
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }
    private SceneManager SceneManager { get; set; }

    public void Init(SceneManager sceneManager)
    {
        SceneManager = sceneManager;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.UseDoorToChangeScene(DoorName, SceneName);
        }
    }

    public void ActivateCameraArea()
    {
        CameraArea.ActivateCamera();
    }
}
