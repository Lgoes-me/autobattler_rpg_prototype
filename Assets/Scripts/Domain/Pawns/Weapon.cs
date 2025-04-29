using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public string Id { get; }
    private int Level { get; }
    private WeaponType Type { get; }
    public List<Sprite> Sprites { get; }
    public Color StartColor { get; }
    public Color EndColor { get; }
    public Sprite Projectile { get; }
    public Stats Stats { get; }
    public AbilityBehaviourData OnHitEffect { get; }

    public Weapon(
        string id,
        int level,
        WeaponType type,
        List<Sprite> sprites,
        Color startColor,
        Color endColor,
        Sprite projectile,
        Stats stats,
        AbilityBehaviourData onHitEffect)
    {
        Id = id;
        Level = level;
        Type = type;
        Sprites = sprites;
        StartColor = startColor;
        EndColor = endColor;
        Projectile = projectile;
        Stats = stats;
        OnHitEffect = onHitEffect;
    }
}