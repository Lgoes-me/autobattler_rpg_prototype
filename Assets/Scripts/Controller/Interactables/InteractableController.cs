using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [field: SerializeField] private CanvasFollowController CanvasFollowController { get; set; }
    [field: SerializeField] private InteractionListener InteractionListener { get; set; }
    
    public void Preselect()
    {
        CanvasFollowController.Show();
        InteractionListener?.PreSelect();
    }

    public void Select()
    {
        InteractionListener?.Interact();
    }

    public void Unselect()
    {
        CanvasFollowController.Hide();
        InteractionListener?.UnSelect();
    }
}