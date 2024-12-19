using System.Collections;
using Cinemachine;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [field:SerializeField] 
    
    public Vector3 ForwardVector { get; private set; }
    public Vector3 RightVector { get; private set; }
    
    private Coroutine Coroutine { get; set; }
    private PlayerManager PlayerManager { get; set; }
    private CinemachineBrain CinemachineBrain { get; set; }
    private CinemachineVirtualCamera VirtualCamera { get; set; }
    
    public void Prepare()
    {
        PlayerManager = Application.Instance.PlayerManager;
        CinemachineBrain = Application.Instance.MainCamera.transform.GetComponent<CinemachineBrain>();
    }

    public void SetNewCameraPosition(CinemachineVirtualCamera virtualCamera, CinemachineBlendDefinition blend)
    {
        CinemachineBrain.m_DefaultBlend = blend;

        virtualCamera.Priority = 10;
        
        if (VirtualCamera != null)
        {
            VirtualCamera.Priority = 0;
        }
        
        VirtualCamera = virtualCamera;
        
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        Coroutine = StartCoroutine(UpdateCameraPositionCoroutine(virtualCamera.transform));
    }

    private IEnumerator UpdateCameraPositionCoroutine(Transform cam)
    {
        yield return new WaitWhile(() => PlayerManager.PlayerController.MoveInput != Vector2.zero);
        
        if (cam == null)
            yield break;

        ForwardVector = Vector3.Dot(cam.forward, -transform.up) > 0.8f ? cam.up : cam.forward;
        RightVector = Vector3.Cross(Vector3.up, ForwardVector).normalized;
    }
}
