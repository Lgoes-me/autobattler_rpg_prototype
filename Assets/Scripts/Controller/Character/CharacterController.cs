using UnityEngine;
using UnityEngine.Rendering;

public class CharacterController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");

    [field: SerializeField] private AnimationStateController AnimationStateController { get; set; }
    [field: SerializeField] private HitStopController HitStopController { get; set; }
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private SortingGroup SortingGroup { get; set; }

    [field: SerializeField] private Transform MountPivot { get; set; }
    [field: SerializeField] private Transform WeaponPivot { get; set; }
    [field: SerializeField] private Transform Pivot { get; set; }
    [field: SerializeField] private WeaponController WeaponController { get; set; }

    public AnimationState CurrentState => AnimationStateController.CurrentState;
    
    private CharacterController MountController { get; set; }

    public void SetDirection(Vector3 direction)
    {
        if(direction.sqrMagnitude < 0.1f)
            return;

        if (MountController != null)
        {
            MountController.SetDirection(direction);
            return;
        }
        
        var xDirection = Vector3.Dot(direction, Application.Instance.MainCamera.transform.right);
        Pivot.localScale = new Vector3(xDirection < -0.2f ? 1 : - 1, 1, 1);
    }

    public void SetSpeed(float speed)
    {
        Animator.SetFloat(Speed, speed);
        
        if (MountController != null)
        {
            MountController.SetSpeed(speed);
        }
    }

    public void SetWeapon(WeaponComponent weaponComponent)
    {
        if (WeaponController != null)
        {
            Destroy(WeaponController.gameObject);
        }
        
        WeaponController = Instantiate(weaponComponent.WeaponPrefab, WeaponPivot);
        WeaponController.SetWeapon(weaponComponent.Weapon);
    }
    
    public void SetMount(CharacterController mountController)
    {
        if (MountController != null)
        {
            Destroy(MountController.gameObject);
        }
        
        MountController = Instantiate(mountController, transform);
        MountController.SetAnimationState(new IdleState());
        
        Pivot.SetParent(MountController.MountPivot);
        
        Pivot.localPosition = Vector3.zero;
        SortingGroup.enabled = false;
    }

    public Sprite GetProjectileSprite()
    {
        return WeaponController.Projectile;
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
        
        if (MountController != null)
        {
            if (state is DeadState || state is IdleState)
            {
                MountController.SetAnimationState(state);
            }
        }
    }
    
    public void PlayFootstep()
    {
        Application.Instance.GetManager<AudioManager>().PlaySound(SfxType.Step);
    }

    public Vector3 GetSpawnPoint()
    {
        return WeaponController.SpawnPoint.position;
    }
}