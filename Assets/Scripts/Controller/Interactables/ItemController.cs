using UnityEngine;

public class ItemController : MonoBehaviour, IInteractable
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