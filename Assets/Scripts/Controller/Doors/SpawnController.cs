using System;
using UnityEngine;

namespace Controller.Doors
{
    public class SpawnController : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] private CameraAreaController CameraArea { get; set; }

        public void SpawnPlayer(PlayerManager playerManager)
        {
            ActivateCameraArea();
            playerManager.PlayerController.transform.position = transform.position;
            playerManager.PawnController.CharacterController.SetDirection(transform.forward);
        }
        
        private void ActivateCameraArea()
        {
            CameraArea.ActivateCamera();
        }
        
        private void OnValidate()
        {
            if(Id == string.Empty)
            {
                Id = Guid.NewGuid().ToString();
            }
        }    }
}