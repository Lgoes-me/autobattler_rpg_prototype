using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour, IInteractable
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
