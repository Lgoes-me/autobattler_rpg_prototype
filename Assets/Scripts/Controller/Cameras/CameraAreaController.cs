using UnityEngine;
using Cinemachine;

public class CameraAreaController : MonoBehaviour
{
    [field: SerializeField] private CinemachineVirtualCamera CinemachineVirtualCamera { get; set; }
    [field: SerializeField] private CinemachineBlendDefinition Blend { get; set; }
    [field: SerializeField] private bool Follow { get; set; }

    private void Awake()
    {
        CinemachineVirtualCamera.Priority = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCamera();
        }
    }
    
    public void ActivateCamera()
    {
        if (Follow)
        {
            CinemachineVirtualCamera.Follow = Application.Instance.GetManager<PlayerManager>().PlayerTransform;
        }

        Application.Instance.GetManager<InputManager>().SetNewCameraPosition(CinemachineVirtualCamera, Blend);
    }
}