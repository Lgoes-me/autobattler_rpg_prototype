using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] private PawnController PawnController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private float Speed { get; set; }

    public Vector2 MoveInput { get; private set; }

    public void Init()
    {
        PawnController.CharacterController.SetAnimationState(new IdleState());
        MoveInput = Vector2.zero;
    }

    private void Update()
    {
        if(PawnController.CharacterController == null)
            return;
        
        /*if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out var hit, 100))
        {
            NavMeshAgent.SetDestination(hit.point);
            Debug.Log ("hit");
        }
        
        PawnController.CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
        */
        
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        PawnController.CharacterController.SetSpeed(MoveInput.magnitude);
    }

    private void FixedUpdate()
    {
        if (MoveInput.sqrMagnitude < Mathf.Epsilon)
            return;

        var inputManager = Application.Instance.GetManager<InputManager>();
        
        var lateralInput = inputManager.RightVector * MoveInput.x;
        var verticalInput = inputManager.ForwardVector * MoveInput.y;
        var input = lateralInput + verticalInput;
        
        input = Vector3.ClampMagnitude(input, 1f);
        
        PawnController.CharacterController.SetDirection(lateralInput);
        
        var destination = transform.position + input * Speed;

        if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            return;

        transform.position = transform.position.Follow(new Vector3(hit.position.x, transform.position.y, hit.position.z), 25);
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

    public async Task MovePlayerTo(Transform destination)
    {
        await this.WaitToArriveAtDestination(NavMeshAgent, destination.position);
    }
}