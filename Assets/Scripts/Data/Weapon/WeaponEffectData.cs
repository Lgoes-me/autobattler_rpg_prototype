using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponEffectData
{
    [field: SerializeField] private EffectType Type { get; set; }
    [field: SerializeField] private List<AbilityBehaviourData> Effects { get; set; }

    public WeaponEffect ToDomain()
    {
        return new WeaponEffect(Type, Effects);
    }
}

public enum EffectType
{
    Unknown,
    InstantAction,
}