using UnityEngine;

public class BaseCanvasController : MonoBehaviour
{
    [field: SerializeField] private GameObject Root { get; set; }

    public virtual void Show()
    {
        Root.SetActive(true);
    }

    public virtual void Hide()
    {
        Root.SetActive(false);
    }
}