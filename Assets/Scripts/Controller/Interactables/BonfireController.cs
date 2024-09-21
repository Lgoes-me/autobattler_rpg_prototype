using System;
using UnityEngine;

public class BonfireController : InteractableStrategy
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }
    private string Scene { get; set; }

    public void Init(string scene)
    {
        Scene = scene;
    }
    
    public override void Interact()
    {
        Application.Instance.SceneManager.StartBonfireScene(Scene, Id);
    }

    public override void UnSelect()
    {
        base.UnSelect();
        Application.Instance.SceneManager.EndBonfireScene();
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