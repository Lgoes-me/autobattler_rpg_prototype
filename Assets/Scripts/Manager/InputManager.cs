using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Coroutine Coroutine { get; set; }

    public Vector3 ForwardVector { get; private set; }
    public Vector3 RightVector { get; private set; }
    
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
        yield return new WaitWhile(() => Application.Instance.PlayerManager.PlayerController.MoveInput != Vector2.zero);
        
        if (cam == null)
            yield break;

        ForwardVector = Vector3.Dot(cam.forward, -transform.up) > 0.8f ? cam.up : cam.forward;
        RightVector = Vector3.Cross(Vector3.up, ForwardVector).normalized;
    }
}
