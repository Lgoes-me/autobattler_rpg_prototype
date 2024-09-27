﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; set; }
    [field: SerializeField] private float Speed { get; set; }

    private Vector2 MoveInput { get; set; }
    private Vector3 ForwardVector { get; set; }
    private Vector3 RightVector { get; set; }
    private Coroutine Coroutine { get; set; }

    
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

        var input = RightVector * MoveInput.x + ForwardVector * MoveInput.y;
        input = Vector3.ClampMagnitude(input, 1f);
        CharacterController.SetDirection(input);
        
        var destination = transform.position + input * Speed;

        if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            return;

        transform.position = transform.position.Follow(new Vector3(hit.position.x, transform.position.y, hit.position.z), 25);
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
        CharacterController.SetSpeed(0);
    }

    private IEnumerator UpdateCameraPositionCoroutine(Transform cam)
    {
        yield return new WaitWhile(() => MoveInput != Vector2.zero);
        
        if (cam == null)
            yield break;

        ForwardVector = Vector3.Dot(cam.forward, -transform.up) > 0.8f ? cam.up : cam.forward;
        RightVector = Vector3.Cross(Vector3.up, ForwardVector).normalized;
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