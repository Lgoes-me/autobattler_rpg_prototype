﻿public class NoResourceComponent : AbilityResourceComponent
{
    public NoResourceComponent(PawnController abilityUser) : base(abilityUser)
    {
    }

    public override bool HasResource() => true;

    public override void SpendResource()
    {
        
    }
}