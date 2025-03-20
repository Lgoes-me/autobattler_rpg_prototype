using System;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }

    public virtual void SpawnPlayer()
    {
        ActivateCameraArea();
        Application.Instance.PlayerManager.SpawnPlayerAt(SpawnPoint);
    }

    private void ActivateCameraArea()
    {
        CameraArea.ActivateCamera();
    }

    private void OnValidate()
    {
        if (Id == string.Empty)
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}