using UnityEngine;

public class CanvasFollowController : BaseCanvasController
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
        if(ToFollow == null)
            return;

        Pivot.position = Camera.WorldToScreenPoint(ToFollow.position);
    }
}
