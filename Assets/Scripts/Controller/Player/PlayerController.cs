﻿using System.Threading.Tasks;
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

        if (!MouseInput)
            return;

        PawnController.CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
    }

    public void SetDestination(Vector3 destination)
    {
        if(!enabled)
            return;
        
        MouseInput = true;
        NavMeshAgent.isStopped = false;
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

        MouseInput = false;
        NavMeshAgent.isStopped = true;
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