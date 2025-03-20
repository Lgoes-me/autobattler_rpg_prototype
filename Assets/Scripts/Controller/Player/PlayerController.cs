using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] private PawnController PawnController { get; set; }
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
        
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        PawnController.CharacterController.SetSpeed(MoveInput.magnitude);
    }

    private void FixedUpdate()
    {
        if (MoveInput.sqrMagnitude < Mathf.Epsilon)
            return;

        var inputManager = Application.Instance.InputManager;
        var input = inputManager.RightVector * MoveInput.x + inputManager.ForwardVector * MoveInput.y;
        input = Vector3.ClampMagnitude(input, 1f);
        PawnController.CharacterController.SetDirection(input);
        
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
}