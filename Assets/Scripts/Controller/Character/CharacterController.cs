using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");

    [field: SerializeField] private AnimationStateController AnimationStateController { get; set; }
    [field: SerializeField] public WeaponController WeaponController { get; private set; }
    [field: SerializeField] private HitStopController HitStopController { get; set; }
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private Transform Hand { get; set; }
    [field: SerializeField] private Transform Pivot { get; set; }

    public AnimationState CurrentState => AnimationStateController.CurrentState;

    public void SetDirection(Vector3 direction)
    {
        if(direction.sqrMagnitude < 0.1f)
            return;

        var xDirection = Vector3.Dot(direction, Application.Instance.MainCamera.transform.right);
        Pivot.localScale = new Vector3(xDirection < -0.2f ? 1 : - 1, 1, 1);
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
}