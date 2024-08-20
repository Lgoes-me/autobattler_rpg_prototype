using UnityEngine;
using Cinemachine;

public class CameraAreaController : MonoBehaviour
{
    [field: SerializeField] private CinemachineVirtualCamera CinemachineVirtualCamera { get; set; }
    [field: SerializeField] private CinemachineBlendDefinition Blend { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovementController>(out var movementController))
        {
            movementController.SetNewCameraPosition(CinemachineVirtualCamera.transform);
            CinemachineVirtualCamera.Priority = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Camera.main is not null) Camera.main.transform.GetComponent<CinemachineBrain>().m_DefaultBlend = Blend;
            CinemachineVirtualCamera.Priority = 0;
        } 
    }
}