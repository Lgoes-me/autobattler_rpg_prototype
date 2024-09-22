using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [field: SerializeField] private string SceneName { get; set; }
    [field: SerializeField] public string DoorName { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] public bool Active { get; set; } = true;
    
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }
    private SceneManager SceneManager { get; set; }
    private SpawnDomain Spawn { get; set; }

    public void Init(SceneManager sceneManager)
    {
        SceneManager = sceneManager;
        Spawn = new SpawnDomain(DoorName, SceneName);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            SceneManager.UseDoorToChangeScene(Spawn);
        }
    }

    public void ActivateCameraArea()
    {
        CameraArea.ActivateCamera();
    }
}
