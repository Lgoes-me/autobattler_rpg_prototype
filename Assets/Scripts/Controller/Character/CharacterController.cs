using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");

    [field: SerializeField] private AnimationStateController AnimationStateController { get; set; }
    [field: SerializeField] public WeaponController WeaponController { get; private set; }
    [field: SerializeField] private HitStopController HitStopController { get; set; }
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private Transform Arm { get; set; }
    [field: SerializeField] private Transform Hand { get; set; }
    [field: SerializeField] private SpriteRenderer Body { get; set; }
    [field: SerializeField] private SpriteMask BodyMask { get; set; }
    [field: SerializeField] private Sprite FrontSprite { get; set; }
    [field: SerializeField] private Sprite BackSprite { get; set; }
    
    private Camera Camera { get; set; }

    private Direction CurrentDirection { get; set; }
    public AnimationState CurrentState => AnimationStateController.CurrentState;

    private void Start()
    {
        Camera = Application.Instance.MainCamera;
        CurrentDirection = Direction.Unknown;
        SetDirection(-transform.forward);
    }

    public void SetDirection(Vector3 direction)
    {
        var zDirection = Vector3.Dot(direction, Camera.transform.forward);
        var xDirection = Vector3.Dot(direction, Camera.transform.right);

        var newDirection = xDirection >= -0.2f ? Direction.Right : Direction.Left;
        newDirection |= zDirection <= 0.2f ? Direction.Front : Direction.Back;

        if (newDirection == CurrentDirection)
            return;

        CurrentDirection = newDirection;

        var left = CurrentDirection.HasFlag(Direction.Left);
        var back = CurrentDirection.HasFlag(Direction.Back);

        Body.flipX = left;
        BodyMask.transform.localRotation = Quaternion.Euler(new Vector3(0, left ? 180 : 0, 0));
        Body.sprite = BodyMask.sprite = back ? BackSprite : FrontSprite;
        Arm.localScale = new Vector3(left ? -1 : 1, 1, back ? -1 : 1);
    }

    public void SetSpeed(float speed)
    {
        Animator.SetFloat(Speed, speed);
    }

    public void DoHitStop()
    {
        HitStopController.HitStop(0f, 0.05f, false, Color.red);
    }

    public void DoNiceHitStop()
    {
        HitStopController.HitStop(0f, 0.05f, false, Color.green);
    }
    
    public void SetAnimationState(AnimationState state)
    {
        AnimationStateController.SetAnimationState(state);
    }

    public void SetWeapon(WeaponController weaponController)
    {
        if (WeaponController != null)
        {
            Destroy(WeaponController.gameObject);
            WeaponController = null;
        }
    
        WeaponController = Instantiate(weaponController, Hand);
    }

    [Flags]
    private enum Direction
    {
        Unknown = 0,
        Right = 1,
        Left = 2,
        Back = 4,
        Front = 8
    }
}