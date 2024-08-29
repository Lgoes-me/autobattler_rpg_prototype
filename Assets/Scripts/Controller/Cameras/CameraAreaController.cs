using UnityEngine;
using Cinemachine;

public class CameraAreaController : MonoBehaviour
{
    [field: SerializeField] private CinemachineVirtualCamera CinemachineVirtualCamera { get; set; }
    [field: SerializeField] private CinemachineBlendDefinition Blend { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCamera();
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

    public void ActivateCamera()
    {
        Application.Instance.PlayerManager.SetNewCameraPosition(CinemachineVirtualCamera.transform);
        CinemachineVirtualCamera.Priority = 10;
    }
}