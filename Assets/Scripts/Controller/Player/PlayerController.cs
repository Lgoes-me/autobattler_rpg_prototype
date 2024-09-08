using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] private AnimationStateController AnimationStateController { get; set; }
    
    private void OnEnable()
    {
        AnimationStateController.SetAnimationState(new IdleState());
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