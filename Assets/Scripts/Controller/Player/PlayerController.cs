using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; set; }
    [field: SerializeField] private float Speed { get; set; }

    public Vector2 MoveInput { get; private set; }

    public void Init()
    {
        CharacterController.SetAnimationState(new IdleState());
        MoveInput = Vector2.zero;
    }

    private void Update()
    {
        if(CharacterController == null)
            return;
        
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        CharacterController.SetSpeed(MoveInput.magnitude);
    }

    private void FixedUpdate()
    {
        if (MoveInput.sqrMagnitude < Mathf.Epsilon)
            return;

        var InputManager = Application.Instance.InputManager;
        var input = InputManager.RightVector * MoveInput.x + InputManager.ForwardVector * MoveInput.y;
        input = Vector3.ClampMagnitude(input, 1f);
        CharacterController.SetDirection(input);
        
        var destination = transform.position + input * Speed;

        if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            return;

        transform.position = transform.position.Follow(new Vector3(hit.position.x, transform.position.y, hit.position.z), 25);
    }

    public void Prepare()
    {
        CharacterController.SetSpeed(0);
    }

    private void OnTriggerEnter(Collider other)
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