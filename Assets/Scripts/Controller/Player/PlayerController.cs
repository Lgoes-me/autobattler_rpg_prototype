using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] private PawnController PawnController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private float Speed { get; set; }
    private bool MouseInput { get; set; }

    public void Init()
    {
        PawnController.CharacterController.SetAnimationState(new IdleState());
    }

    private void Update()
    {
        if (PawnController.CharacterController == null)
            return;

        if (NavMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            return;

        var moveInput = Application.Instance.GetManager<InputManager>().MoveInput;
        PawnController.CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude + moveInput.magnitude);
        PawnController.CharacterController.SetDirection(NavMeshAgent.velocity);
    }

    public void SetDestination(Vector3 destination)
    {
        if (!enabled)
            return;

        MouseInput = true;
        NavMeshAgent.SetDestination(destination);
    }

    private void FixedUpdate()
    {
        var moveInput = Application.Instance.GetManager<InputManager>().MoveInput;

        if (moveInput.sqrMagnitude < Mathf.Epsilon)
        {
            if (!MouseInput)
            {
                PawnController.CharacterController.SetSpeed(0);
            }

            return;
        }

        if (MouseInput)
        {
            NavMeshAgent.ResetPath();
            NavMeshAgent.velocity = Vector3.zero;
        }

        MouseInput = false;
        PawnController.CharacterController.SetSpeed(moveInput.magnitude);

        var inputManager = Application.Instance.GetManager<InputManager>();

        var lateralInput = inputManager.RightVector * moveInput.x;
        var verticalInput = inputManager.ForwardVector * moveInput.y;
        var input = lateralInput + verticalInput;

        input = Vector3.ClampMagnitude(input, 1f);

        PawnController.CharacterController.SetDirection(lateralInput);

        var destination = transform.position + input * Speed;

        if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            return;

        transform.position =
            transform.position.Follow(new Vector3(hit.position.x, transform.position.y, hit.position.z), 25);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<InteractableController>(out var interactable))
        {
            interactable.Preselect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InteractableController>(out var interactable))
        {
            interactable.Unselect();
        }
    }

    public Task MovePlayerTo(Transform destination)
    {
        var task = new TaskCompletionSource<bool>();

        StartCoroutine(WaitToArriveAtDestinationCoroutine());

        return task.Task;

        IEnumerator WaitToArriveAtDestinationCoroutine()
        {
            var time = Time.time;

            var acceleration = NavMeshAgent.acceleration;
            NavMeshAgent.acceleration = 1000;
            NavMeshAgent.SetDestination(destination.position);

            yield return new WaitForEndOfFrame();

            var inputManager = Application.Instance.GetManager<InputManager>();

            while (Time.time - time <= 3 && 
                  (NavMeshAgent.pathStatus != NavMeshPathStatus.PathComplete || NavMeshAgent.remainingDistance >= 0.1f) &&
                   inputManager.MoveInput.sqrMagnitude <= Mathf.Epsilon &&
                   !MouseInput)
            {
                NavMeshAgent.SetDestination(destination.position);
                yield return new WaitForSeconds(0.1f);
            }

            NavMeshAgent.acceleration = acceleration;
            NavMeshAgent.velocity = Vector3.zero;
            NavMeshAgent.ResetPath();

            task.SetResult(true);
        }
    }

    public void Disable()
    {
        enabled = false;
        NavMeshAgent.ResetPath();
        NavMeshAgent.velocity = Vector3.zero;
    }

    public void Enable()
    {
        enabled = true;
    }
}