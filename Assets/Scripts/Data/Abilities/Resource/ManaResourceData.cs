﻿using UnityEngine;

[System.Serializable]
public class ManaResourceData : ResourceData
{
    [field: SerializeField] private int ManaCost { get; set; }

    public override AbilityResourceComponent ToDomain(PawnController abilityUser)
    {
        return new ManaResourceComponent(abilityUser, ManaCost);
    }

    public override int GetCost() => ManaCost;
}