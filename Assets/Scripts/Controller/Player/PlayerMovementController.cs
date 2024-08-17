using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{
    //Domain
    [field: SerializeField] private float Speed { get; set; }

    private Vector2 MoveInput { get; set; }
    private Matrix4x4 Matrix { get; set; }
    private Coroutine Coroutine { get; set; }

    private void Start()
    {
        MoveInput = Vector2.zero;
    }

    private void Update()
    {
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        if (MoveInput.sqrMagnitude < Mathf.Epsilon)
            return;

        var input = new Vector3(MoveInput.x, 0, MoveInput.y);
        input = Vector3.ClampMagnitude(input, 1f);
        input = Matrix.MultiplyVector(input);

        var destination = transform.position + input * Speed;

        if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            return;

        transform.position = transform.position.Follow(new Vector3(hit.position.x, transform.position.y, hit.position.z), 25);
        transform.rotation = transform.rotation.Rotate(Quaternion.LookRotation(new Vector3(input.x, 0, input.z), transform.up), 25);
    }

    public void SetNewCameraPosition(Transform cam)
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        Coroutine = StartCoroutine(UpdateCameraPositionCoroutine(cam));
    }

    private IEnumerator UpdateCameraPositionCoroutine(Transform cam)
    {
        yield return new WaitWhile(() => MoveInput != Vector2.zero);
        Matrix = Matrix4x4.TRS(cam.position, cam.rotation, Vector3.one);
    }
}