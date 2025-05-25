using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public WeaponType Type { get; private set; }
    [field: SerializeField] private List<Sprite> Sprites { get; set; }
    [field: SerializeField] private Color StartColor { get; set; }
    [field: SerializeField] private Color EndColor { get; set; }
    [field: SerializeField] private Sprite Projectile { get; set; }
    [field: SerializeField] private StatsData Stats { get; set; }
    [field: SerializeField] private List<WeaponEffectData> WeaponEffects { get; set; }
    [field: SerializeField] private WeaponController WeaponPrefab { get; set; }

    public Weapon ToDomain()
    {
        var weaponEffects = WeaponEffects.Select(w => w.ToDomain()).ToList();
        return new Weapon(Id, Level, Type, Sprites, StartColor, EndColor, Projectile, Stats.ToDomain(), weaponEffects, WeaponPrefab);
    }
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