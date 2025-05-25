using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public string Id { get; }
    private int Level { get; }
    public WeaponType Type { get; }
    public List<Sprite> Sprites { get; }
    public Color StartColor { get; }
    public Color EndColor { get; }
    public Sprite Projectile { get; }
    public Stats Stats { get; }
    public List<WeaponEffect> WeaponEffects { get; }

    public Weapon(
        string id,
        int level,
        WeaponType type,
        List<Sprite> sprites,
        Color startColor,
        Color endColor,
        Sprite projectile,
        Stats stats,
        List<WeaponEffect> weaponEffects)
    {
        Id = id;
        Level = level;
        Type = type;
        Sprites = sprites;
        StartColor = startColor;
        EndColor = endColor;
        Projectile = projectile;
        Stats = stats;
        WeaponEffects = weaponEffects;
    }
}