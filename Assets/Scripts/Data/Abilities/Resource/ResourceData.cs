﻿[System.Serializable]
public abstract class ResourceData : IComponentData
{
    public abstract AbilityResourceComponent ToDomain(PawnController abilityUser);
    public abstract bool HasResource(PawnController abilityUser);
    public abstract int GetCost();
}