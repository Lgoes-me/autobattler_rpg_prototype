using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer[] SpriteRenderers { get; set; }
    [field: SerializeField] private TrailRenderer TrailRenderer { get; set; }
    
    public Sprite Projectile { get; private set; }
    
    public void SetWeapon(WeaponData weapon)
    {
        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderers[i].sprite = weapon.Sprites[i];
        }
        
        TrailRenderer.startColor = weapon.StartColor;
        TrailRenderer.endColor = weapon.EndColor;
        
        Projectile = weapon.Projectile;
    }
}