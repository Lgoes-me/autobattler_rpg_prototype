using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public PawnData PawnData { get; private set; }

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