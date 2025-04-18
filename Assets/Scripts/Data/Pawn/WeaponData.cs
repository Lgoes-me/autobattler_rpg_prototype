using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    [field: SerializeField]
    public string Id { get; private set; }
    
    [field: SerializeField]
    public int Level { get; private set; }
    
    [field: SerializeField]
    public WeaponType Type { get; private set; }
    
    [field: SerializeField]
    public List<Sprite> Sprites { get; private set; }
    
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
    Bow = 1,
    Lance = 2,
    Sword = 4,
    Hammer = 8,
    Crossbow = 16,
}