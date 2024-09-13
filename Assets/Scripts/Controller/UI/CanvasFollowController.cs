using UnityEngine;

public class CanvasFollowController : MonoBehaviour
{
    [field: SerializeField] private RectTransform Pivot { get; set; }
    [field: SerializeField] private Transform ToFollow { get; set; }
    
    private Camera Camera { get; set; }

    private void Awake()
    {
        Camera = Camera.main;
    }

    private void LateUpdate()
    {
        if(ToFollow == null || !gameObject.activeInHierarchy || !ToFollow.gameObject.activeInHierarchy)
            return;

        Pivot.position = Camera.WorldToScreenPoint(ToFollow.position);
    }
}
