using System;
using UnityEngine;

public class BonfireController : InteractableStrategy
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }
    private SceneManager SceneManager { get; set; }
    private SpawnDomain Spawn { get; set; }

    public void Init(SceneManager sceneManager, string scene)
    {
        SceneManager = sceneManager;
        Spawn = new SpawnDomain(Id, scene);
    }
    
    public override void Interact()
    {
        SceneManager.StartBonfireScene(Spawn);
    }

    public override void UnSelect()
    {
        base.UnSelect();
        SceneManager.EndBonfireScene();
    }
    
    public void ActivateCameraArea()
    {
        CameraArea.ActivateCamera();
    }
    
    private void OnValidate()
    {
        if(Id != string.Empty)
            return;
        
        Id = Guid.NewGuid().ToString();
    }
}