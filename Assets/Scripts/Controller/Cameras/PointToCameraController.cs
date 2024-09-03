using UnityEngine;

public class PointToCameraController : MonoBehaviour
{
    private Camera Camera { get; set; }
    
    private void Awake()
    {
        Camera = Camera.main;
    }

    void FixedUpdate()
    {
        transform.right = Camera.transform.right;
    }
}
