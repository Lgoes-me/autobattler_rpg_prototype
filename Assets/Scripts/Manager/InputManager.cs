using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector3 ForwardVector { get; private set; }
    public Vector3 RightVector { get; private set; }
    
    private Coroutine Coroutine { get; set; }
    private PlayerManager PlayerManager { get; set; }

    private void Start()
    {
        PlayerManager = Application.Instance.PlayerManager;
    }

    public void SetNewCameraPosition(Transform cameraTransform)
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        Coroutine = StartCoroutine(UpdateCameraPositionCoroutine(cameraTransform));
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
