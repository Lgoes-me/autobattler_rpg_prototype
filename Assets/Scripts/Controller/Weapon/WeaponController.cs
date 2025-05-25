using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [field: SerializeField] private List<SpriteRenderer> SpriteRenderers { get; set; }
    [field: SerializeField] private TrailRenderer TrailRenderer { get; set; }
    
    public Sprite Projectile { get; private set; }
    
    public void SetWeapon(Weapon weapon)
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