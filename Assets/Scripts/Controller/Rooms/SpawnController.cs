using System;
using Cinemachine;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }
    
    [field: SerializeField] protected Transform SpawnPoint { get; private set; }
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }

    public virtual void SpawnPlayer(CinemachineBlendDefinition blend)
    {
        Application.Instance.GetManager<PartyManager>().SpawnPartyAt(SpawnPoint.position);
        CameraArea.ActivateCamera(blend);
    }

    private void OnValidate()
    {
        if (Id == string.Empty)
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}