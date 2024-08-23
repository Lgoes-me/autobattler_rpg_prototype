using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{
    [field: SerializeField] private Animator Animator { get; set; }

    //Domain
    [field: SerializeField] private float Speed { get; set; }

    private Vector2 MoveInput { get; set; }

    private Vector3 ForwardVector { get; set; }
    private Vector3 RightVector { get; set; }
    private Coroutine Coroutine { get; set; }

    private void Start()
    {
        MoveInput = Vector2.zero;
    }

    private void Update()
    {
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Animator.SetFloat("Speed", MoveInput.magnitude);
    }

    private void FixedUpdate()
    {
        if (MoveInput.sqrMagnitude < Mathf.Epsilon)
            return;

        var input = RightVector * MoveInput.x + ForwardVector * MoveInput.y;
        input = Vector3.ClampMagnitude(input, 1f);

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

    public void Prepare()
    {
        Animator.SetFloat("Speed", 0);
    }

    private IEnumerator UpdateCameraPositionCoroutine(Transform cam)
    {
        yield return new WaitWhile(() => MoveInput != Vector2.zero);
        
        if (cam == null)
            yield break;

        ForwardVector = Vector3.Dot(cam.forward, -transform.up) > 0.8f ? cam.up : cam.forward;
        RightVector = Vector3.Cross(Vector3.up, ForwardVector).normalized;
    }
}