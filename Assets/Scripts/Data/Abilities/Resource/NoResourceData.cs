
[System.Serializable]
public class NoResourceData : ResourceData
{
    public override AbilityResourceComponent ToDomain(PawnController abilityUser)
    {
        return new NoResourceComponent(abilityUser);
    }

    public override bool HasResource(PawnController abilityUser)
    {
        return true;
    }

    public override int GetCost() => 0;
}