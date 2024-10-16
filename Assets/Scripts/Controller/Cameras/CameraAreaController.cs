using UnityEngine;
using Cinemachine;

public class CameraAreaController : MonoBehaviour
{
    [field: SerializeField] private CinemachineVirtualCamera CinemachineVirtualCamera { get; set; }
    [field: SerializeField] private CinemachineBlendDefinition Blend { get; set; }

    private CinemachineBrain CinemachineBrain { get; set; }
    
    private void Awake()
    {
        CinemachineBrain = Application.Instance.MainCamera.transform.GetComponent<CinemachineBrain>();
    }

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
            CinemachineBrain.m_DefaultBlend = Blend;
            CinemachineVirtualCamera.Priority = 0;
        } 
    }

    public void ActivateCamera()
    {
        Application.Instance.InputManager.SetNewCameraPosition(CinemachineVirtualCamera.transform);
        CinemachineVirtualCamera.Priority = 10;
    }
}