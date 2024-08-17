using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public PawnData PawnData { get; private set; }
    private IInteractable SelectedInteractable { get; set; }
    private List<IInteractable> Interactables { get; set; }
    
    private bool WantsToInteract { get; set; }

    private void Start()
    {
        Interactables = new List<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            SelectedInteractable?.Unselect();

            SelectedInteractable = interactable;
            SelectedInteractable.Preselect();
            
            Interactables.Add(interactable);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (Interactables.Contains(interactable))
            {
                Interactables.Remove(interactable);
            }
            
            if (SelectedInteractable == interactable)
            {
                SelectedInteractable.Unselect();
                SelectedInteractable = Interactables.LastOrDefault();
                SelectedInteractable?.Preselect();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            WantsToInteract = true;
        }
    }

    private void FixedUpdate()
    {
        if (WantsToInteract)
        {
            WantsToInteract = false;
            SelectedInteractable.Select();
        }
    }
}