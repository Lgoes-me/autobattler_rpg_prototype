using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [field: SerializeField] private Transform Arm { get; set; }
    [field: SerializeField] private SpriteRenderer Body { get; set; }
    [field: SerializeField] private SpriteMask BodyMask { get; set; }
    [field: SerializeField] private Sprite FrontSprite { get; set; }
    [field: SerializeField] private Sprite BackSprite { get; set; }

    private Camera Camera { get; set; }
    
    private void Awake()
    {
        Camera = Camera.main;
    }

    public void SetDirection(Vector3 movementVector)
    {
        var zDirection = Vector3.Dot(movementVector, Camera.transform.forward);
        var xDirection = Vector3.Dot(movementVector, Camera.transform.right);
        
        Body.flipX = xDirection < 0;
        Body.sprite = zDirection > 0.2f  ? BackSprite : FrontSprite;
        BodyMask.sprite = zDirection > 0.2f  ? BackSprite : FrontSprite;
        Arm.localScale = new Vector3(xDirection < 0 ? -1 : 1, 1, zDirection > 0.2f ? -1 : 1);
    }
}