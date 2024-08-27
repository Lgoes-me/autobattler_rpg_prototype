using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [field: SerializeField] private CanvasFollowController CanvasFollowController { get; set; }
    
    public void Preselect()
    {
        CanvasFollowController.Show();
    }

    public void Select()
    {
        Debug.Log($"Selected {name}");
    }

    public void Unselect()
    {
        CanvasFollowController.Hide();
    }
}