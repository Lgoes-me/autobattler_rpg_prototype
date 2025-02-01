﻿using System;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] private CameraAreaController CameraArea { get; set; }

    public virtual void SpawnPlayer(PlayerManager playerManager)
    {
        ActivateCameraArea();
        
        playerManager.NavMeshAgent.enabled = false;
        playerManager.PlayerController.transform.position = SpawnPoint.position;
        playerManager.PawnController.CharacterController.SetDirection(SpawnPoint.forward);
        playerManager.NavMeshAgent.enabled = true;
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