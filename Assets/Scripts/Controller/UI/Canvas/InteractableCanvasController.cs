using UnityEngine;
using UnityEngine.UI;

public class InteractableCanvasController : BaseCanvasController
{
    [field: SerializeField] private Button Button { get; set; }

    public void Init(InteractableController interactableController)
    {
        Button.onClick.AddListener(interactableController.Select);
    }
}