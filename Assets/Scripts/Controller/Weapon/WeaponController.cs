using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [field: SerializeField] private List<SpriteRenderer> SpriteRenderers { get; set; }
    [field: SerializeField] private TrailRenderer TrailRenderer { get; set; }
    
    public Sprite Projectile { get; private set; }
    
    public void SetWeapon(WeaponData weapon)
    {
        for (var i = 0; i < SpriteRenderers.Count; i++)
        {
            SpriteRenderers[i].sprite = weapon.Sprites[i];
        }
        
        TrailRenderer.startColor = weapon.StartColor;
        TrailRenderer.endColor = weapon.EndColor;
        
        Projectile = weapon.Projectile;
    }
}