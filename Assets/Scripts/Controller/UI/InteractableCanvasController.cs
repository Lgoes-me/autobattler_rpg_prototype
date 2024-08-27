using UnityEngine;
using UnityEngine.UI;

public class InteractableCanvasController : MonoBehaviour
{
    [field: SerializeField] private InteractableController InteractableController { get; set; }
    [field: SerializeField] private Button Button { get; set; }

    private void Start()
    {
        Button.onClick.AddListener(() => InteractableController.Select());
    }
}