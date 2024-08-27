using UnityEngine;

public class CanvasFollowController : MonoBehaviour
{
    [field: SerializeField] private RectTransform Pivot { get; set; }
    [field: SerializeField] private Transform ToFollow { get; set; }
    private Camera Camera { get; set; }

    private void Start()
    {
        Camera = Camera.main;
    }

    private void LateUpdate()
    {
        if(ToFollow == null)
            return;

        Pivot.position = Camera.WorldToScreenPoint(ToFollow.position);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
