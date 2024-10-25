using System;
using UnityEngine;

public class BonfireController : InteractableController
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }
    
    private SpawnDomain Spawn { get; set; }
    private bool Selected { get; set; }

    public void Init(string scene)
    {
        Spawn = new SpawnDomain(Id, scene);
    }
    
    protected override void InternalSelect()
    {
        Selected = true;
        Application.Instance.SceneManager.StartBonfireScene(this, Spawn);
    }

    protected override void InternalUnSelect()
    {
        Application.Instance.SceneManager.EndBonfireScene();
        Selected = false;
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