using System;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    [field: SerializeField]
    public WeaponType Type { get; private set; }
    
    [field: SerializeField]
    public Sprite[] Sprites { get; private set; }
    
    [field: SerializeField]
    public Color StartColor { get; private set; }
    
    [field: SerializeField]
    public Color EndColor { get; private set; }
    
    [field: SerializeField]
    public Sprite Projectile { get; private set; }
}

[Flags]
public enum WeaponType
{
    None = 0,
    
    
}