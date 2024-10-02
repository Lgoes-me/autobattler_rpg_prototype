using UnityEngine;

public class PointToCameraController : MonoBehaviour
{
    private Camera Camera { get; set; }
    private Vector3 Right { get; set; }
    
    private void Awake()
    {
        Camera = Camera.main;
    }

    void FixedUpdate()
    {
        if(Right == Camera.transform.right)
            return;
        
        Right = transform.right = Camera.transform.right;
    }
}
