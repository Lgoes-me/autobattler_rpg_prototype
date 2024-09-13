using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [field: SerializeField] private InteractableCanvasController InteractableCanvas { get; set; }
    [field: SerializeField] private InteractableStrategy Interactable { get; set; }

    private void Awake()
    {
        InteractableCanvas.Init(this);
    }

    public void Preselect()
    {
        InteractableCanvas.Show();
        Interactable?.PreSelect();
    }

    public void Select()
    {
        Interactable?.Interact();
    }

    public void Unselect()
    {
        InteractableCanvas.Hide();
        Interactable?.UnSelect();
    }
}